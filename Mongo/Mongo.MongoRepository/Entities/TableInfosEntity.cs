// <copyright file="TableInfosEntity.cs" company="Eureka">
// Copyright (c) Eureka. All rights reserved.
// </copyright>

namespace Mongo.MongoRepository.Entities
{
    using global::MongoRepository;

    using MongoDB.Bson.Serialization.Attributes;
    using System;

    public class TableInfosEntity : Entity
    {
        /// <summary>
        /// 标识编号
        /// </summary>
        public long IdentityNo { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public string VersionName { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public string VersionNo { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 是否初始化
        /// </summary>
        public bool IsInit { get; set; }

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
        /// 租户ID
        /// </summary>
        public string TenantID { get; set; }

        /// <summary>
        /// 前缀
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Suffix { get; set; }

        /// <summary>
        /// 归属类型1.国标2.行标3.院标4.其它
        /// </summary>
        public int BelongType { get; set; }

        /// <summary>
        /// 数据类型 1.元素据类型2.业务数据类型3.其它数据类型
        /// </summary>
        public int DataType { get; set; }

        /// <summary>
        /// 表别名简拼
        /// </summary>
        public string SimpleSpell { get; set; }

        /// <summary>
        /// 历史版本数
        /// </summary>
        public int HistoryCount { get; set; }

        /// <summary>
        /// 版本类型
        /// </summary>
        public int VersionType { get; set; }

        /// <summary>
        /// 当前版本ID
        /// </summary>
        public long CurrentIdentityNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
