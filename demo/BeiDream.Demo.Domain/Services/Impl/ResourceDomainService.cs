﻿using System;
using System.Collections.Generic;
using System.Linq;
using BeiDream.Core.Linq.Extensions;
using BeiDream.Demo.Domain.Model;
using BeiDream.Demo.Domain.Queries;
using BeiDream.Demo.Domain.Repositories;
using BeiDream.Demo.Domain.Services.Contracts;
using BeiDream.Utils.PagerHelper;

namespace BeiDream.Demo.Domain.Services.Impl
{
    public class ResourceDomainService : IResourceDomainService
    {
        /// <summary>
        ///角色仓储
        /// </summary>
        public IResourceRepository ResourceRepository { get; set; }
        public ResourceDomainService(IResourceRepository resourceRepository)
        {
            ResourceRepository = resourceRepository;
        }
        public PagerList<Resource> Query(ResourceQuery query)
        {
            if (string.IsNullOrWhiteSpace(query.Order))   //分页必须先进行排序
                query.Order = "Id desc";
            query.TotalCount = ResourceRepository.GetAll().Count();
            IQueryable<Resource> roles = GetQueryConditions(ResourceRepository.GetAll(), query)   //where查询条件必须放在排序和分页前，不然生成SQL有BUG
                .OrderByIfOrderNullOrEmpty(query.Order)
                    .Skip(query.GetSkipCount())
                    .Take(query.PageSize);
            var result = new PagerList<Resource>(query);
            result.AddRange(roles.ToList());
            return result;
        }

        public List<Resource> QueryAll()
        {
            return ResourceRepository.GetAll().ToList();
        }

        public Resource Find(Guid id)
        {
            return ResourceRepository.Find(id);
        }

        /// <summary>
        /// 构造前台传递的查询条件
        /// </summary>
        /// <param name="queryable"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        private IQueryable<Resource> GetQueryConditions(IQueryable<Resource> queryable, ResourceQuery query)
        {
            if (!string.IsNullOrWhiteSpace(query.Name))
                queryable = queryable.Where(p => p.Name.Contains(query.Name));
            if (query.Enabled != null)
                queryable = queryable.Where(p => p.Enabled == query.Enabled);
            return queryable;
        }
    }
}