// <copyright file="CodeRoleEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    /// <summary>
    /// 编码规则
    /// </summary>
    public class CodeRoleEntity:Entity
    {
        /// <summary>
        /// 表标识编号
        /// </summary>
        public long TableIdentityNo { get; set; }

        /// <summary>
        /// 规则ID
        /// </summary>
        public string CodeRoleID { get; set; }

        /// <summary>
        /// 规则名称
        /// </summary>
        public string CodeRoleName { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 不定长
        /// </summary>
        public bool Unsized { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public int RoleType { get; set; }

        /// <summary>
        /// 格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 补位字符
        /// </summary>
        public char PadChar { get; set; } 

        /// <summary>
        /// 可以根据此编码分组
        /// </summary>
        public bool IsGroup { get; set; }
    }
}
