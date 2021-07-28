// <copyright file="ColEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using MongoDB.Bson.Serialization.Attributes;

    using System;

    /// <summary>
    /// 列实体
    /// </summary>
    public class ColEntity : Entity
    {
        /// <summary>
        /// 表标识编号
        /// </summary>
        public long TableIdentityNo { get; set; }

        /// <summary>
        /// 列编号
        /// </summary>
        public long ColIdentityNo { get; set; }

        /// <summary>
        /// 列名
        /// </summary>
        public string ColName { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string ColAliasName { get; set; }

        /// <summary>
        /// 数据类型 varchar(50)
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 子类型
        /// </summary>
        public int SubType { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 是否标识
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// 是否唯一
        /// </summary>
        public bool IsUnique { get; set; }

        /// <summary>
        /// 是否分组字段
        /// </summary>
        public bool IsGroup { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否展示(用于前端动态生成form，CSV表头)
        /// </summary>
        public bool IsShow { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string Creater { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string Updater { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 是否组合唯一
        /// </summary>
        public bool IsComPositeUnique { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public bool OrderCol { get; set; }

        /// <summary>
        /// 是否升序
        /// </summary>
        public bool IsAsc { get; set; }

        /// <summary>
        /// 序号
        /// </summary>
        public int SeqNo { get; set; }

        /// <summary>
        /// 首拼搜索列
        /// </summary>
        public bool FirstSpellSearch { get; set; }

        /// <summary>
        /// 列别名简拼
        /// </summary>
        public string SimpleSpell { get; set; }
    }
}
