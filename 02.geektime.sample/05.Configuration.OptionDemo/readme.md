#### 选项接口
#### IOptions<TOptions>:
+ 不*支持： 在应用启动后读取配置数据。
命名选项
+ 注册为单一实例且可以注入到任何服务生存期。
#### IOptionsSnapshot<TOptions>:
+ 在每次请求时应重新计算选项的方案中有用。 有关详细信息，请参阅使用 IOptionsSnapshot 读取已更新的数据。
+ 注册为范围内，因此无法注入到单一实例服务。
+ 支持命名选项
#### IOptionsMonitor<TOptions>:
+ 用于检索选项并管理 TOptions 实例的选项通知。
+ 注册为单一实例且可以注入到任何服务生存期。
##### 支持：
+ 更改通知
+ 命名选项
+ 可重载配置
+ 选择性选项失效 (IOptionsMonitorCache<TOptions>)


#### IOptionsMonitor 和 IOptionsSnapshot 之间的区别在于：
+ IOptionsMonitor 是一种单一示例服务，可随时检索当前选项值，这在单一实例依赖项中尤其有用。
+ IOptionsSnapshot 是一种作用域服务，并在构造 IOptionsSnapshot<T> 对象时提供选项的快照。 选项快照旨在用于暂时性和有作用域的依赖项。

#### 对配置添加验证启动让程序异常
+ 属性验证
+ 自定义验证类（需要继承```IValidateOptions```）