// <copyright file="UploadResultEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using MongoDB.Bson.Serialization.Attributes;
    using System;

    /// <summary>
    /// 上传结果实体
    /// </summary>
    public class UploadResultEntity : Entity
    {
        public long BusinessID { get; set; }
        /// <summary>
        /// 表标识编号
        /// </summary>
        public long TableIdentityNo { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 上传时间
        /// </summary>
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime UploadTime { get; set; }

        /// <summary>
        /// 上传结果
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 文件流
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 结构
        /// </summary>
        public string Structure { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
