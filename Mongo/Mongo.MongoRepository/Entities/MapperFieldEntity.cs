// <copyright file="MapperFiledEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using System.Collections.Generic;

    /// <summary>
    /// 映射字段实体
    /// </summary>
    public class MapperFieldEntity : Entity
    {
        /// <summary>
        /// 映射器ID
        /// </summary>
        public string MapperID { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段别名
        /// </summary>
        public string FieldAlias { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string FieldType { get; set; }

        /// <summary>
        /// 数据匹配方法
        /// </summary>
        public int DataMapperType { get; set; }

        /// <summary>
        /// 列列表
        /// </summary>
        public List<string> ColFullNames { get; set; }

        /// <summary>
        /// 字段表达式
        /// </summary>
        public string FieldExpression { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
