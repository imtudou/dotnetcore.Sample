// <copyright file="MapperRelationEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using MongoRepository;

    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// 映射关系
    /// </summary>
    [DataContract]
    public class MapperRelationEntity : Entity
    {
        /// <summary>
        /// 映射ID
        /// </summary>
        [DataMember]
        public string MapperID { get; set; }

        /// <summary>
        /// 关系ID
        /// </summary>
        [DataMember]
        public string RelationID { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [DataMember]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        [DataMember]
        public string FieldType { get; set; }

        /// <summary>
        /// 关系类型
        /// </summary>
        [DataMember]
        public int RelationType { get; set; }

        /// <summary>
        /// 关联表名称
        /// </summary>
        [DataMember]
        public string RelationTableName { get; set; }

        /// <summary>
        /// 关联字段名称
        /// </summary>
        [DataMember]
        public string RelationFieldName { get; set; }

        /// <summary>
        /// 关联字段类型
        /// </summary>
        [DataMember]
        public string RelationFieldType { get; set; }

        /// <summary>
        /// 关系表达式
        /// </summary>
        [DataMember]
        public string RuleExpression { get; set; }

        /// <summary>
        /// 规则列表
        /// </summary>
        [DataMember]
        public List<RuleItem> RuleItems { get; set; }
    }

    /// <summary>
    /// 规则
    /// </summary>
    [DataContract]
    public class RuleItem
    {
        /// <summary>
        /// 表名称
        /// </summary>
        [DataMember]
        public string TableName { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [DataMember]
        public string FieldName { get; set; }

        /// <summary>
        /// 规则类型
        /// </summary>
        [DataMember]
        public int RuleType { get; set; }

        /// <summary>
        /// 规则参数
        /// </summary>
        [DataMember]
        public string[] RuleParams { get; set; }
    }
}
