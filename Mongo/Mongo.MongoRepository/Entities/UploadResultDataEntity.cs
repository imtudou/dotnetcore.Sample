// <copyright file="UploadResultDataEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using MongoDB.Bson.Serialization.Attributes;
    using System;

    /// <summary>
    /// 上传结果数据
    /// </summary>
    public class UploadResultDataEntity : Entity
    {
        /// <summary>
        /// ID
        /// </summary>
        public long BusinessID { get; set; }

        /// <summary>
        /// 上传数据
        /// </summary>
        public string UploadData { get; set; }

        /// <summary>
        /// 结果
        /// </summary>
        public bool UploadResult { get; set; }

        /// <summary>
        /// 原因
        /// </summary>
        public string Reson { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; }
    }
}
