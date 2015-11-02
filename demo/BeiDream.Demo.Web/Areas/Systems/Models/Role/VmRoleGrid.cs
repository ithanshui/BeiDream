﻿using System;
using System.ComponentModel.DataAnnotations;

namespace BeiDream.Demo.Web.Areas.Systems.Models.Role
{
    public class VmRoleGrid
    {
        public Guid Id { get; set; }
        public virtual string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 是否超级管理员
        /// </summary>
        public bool IsAdmin { get; set; }
        /// <summary>
        /// 启用
        /// </summary>
        public bool Enabled { get; set; }
        public virtual string DateCreated
        {
            get;
            set;
        }
        /// <summary>
        /// 选中（当前角色是否属于选择的用户）
        /// </summary>
        public bool Checked { get; set; }
    }
}