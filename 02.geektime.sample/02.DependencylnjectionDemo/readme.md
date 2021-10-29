## 依赖注入
#### 组件包
+ Microsoft.Extensions.DependencyInjection
+ Microsoft.Extensions.DependencyInjection.Abstractions

#### 核心类型
+ IServicCollection(服务注册)
+ ServiceDescriptor（每一个服务注册时的信息）
+ IServiceProvider（具体的容器）
+ IServiceScope（一个容器子容器的生命周期）

#### 生命周期（ServiceLifetime）

+ Singleton(单例)
+ Scoped(作用域)
+ TranSient(瞬时：每一次去容器里面获取对象时都可以得到一个全新的对象)