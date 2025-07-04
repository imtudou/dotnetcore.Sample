// <Copyright file="RefConstraintEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

using MongoRepository;

namespace Mongo.MongoRepository.Entities
{                                       

    /// <summary>
    /// 引用约束
    /// </summary>
    public class RefConstraintEntity: Entity
    {
        /// <summary>
        /// 约束ID
        /// </summary>
        public string ConstraintID { get; set; }

        /// <summary>
        /// 表标识编号
        /// </summary>
        public long TableIdentityNo { get; set; }

        /// <summary>
        /// 列编号
        /// </summary>
        public long ColIdentityNo { get; set; }

        /// <summary>
        /// 引用表标识编号
        /// </summary>
        public long RefTableIdentityNo { get; set; }

        /// <summary>
        /// 引用列编号
        /// </summary>
        public long RefColIdentityNo { get; set; }

        /// <summary>
        /// 引用表名
        /// </summary>
        public string RefTableName { get; set; }

        /// <summary>
        /// 引用列名
        /// </summary>
        public string RefColName { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
