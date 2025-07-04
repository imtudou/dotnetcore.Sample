// <copyright file="MapperEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using System;

    /// <summary>
    /// 映射器实体
    /// </summary>
    public class MapperEntity:Entity
    {
        /// <summary>
        /// 映射器ID
        /// </summary>
        public string MapperID { get; set; }

        /// <summary>
        /// 映射器名称
        /// </summary>
        public string MapperName { get; set; }

        /// <summary>
        /// 有效开始时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EffectiveStartTime { get; set; }

        /// <summary>
        /// 有效结束时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EffectiveEndTime { get; set; }

        /// <summary>
        /// 映射类型
        /// </summary>
        public int MapperType { get; set; }

        /// <summary>
        /// 主表
        /// </summary>
        public string MainTable { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string MapperDesc { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [MongoDB.Bson.Serialization.Attributes.BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime LastUpdateDate { get; set; }

        /// <summary>
        /// 最后更新人
        /// </summary>
        public string LastUpdateUser { get; set; }

        /// <summary>
        /// 更新标记
        /// </summary>
        public bool UpdateFlag { get; set; }

        /// <summary>
        /// 匹配结果表
        /// </summary>
        public string MapperResultTable { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
