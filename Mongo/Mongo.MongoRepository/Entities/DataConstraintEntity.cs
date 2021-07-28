// <copyright file="DataConstraintEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>



namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    /// <summary>
    /// 内容约束
    /// </summary>
    public class DataConstraintEntity:Entity
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
        /// 约束类型
        /// </summary>
        public string ConstraintType { get; set; }

        /// <summary>
        /// 约束值
        /// </summary>
        public string ConstraintValue { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
