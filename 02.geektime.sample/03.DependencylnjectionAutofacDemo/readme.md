
参考：
+ https://autofaccn.readthedocs.io/en/latest/integration/aspnetcore.html
+ https://www.cnblogs.com/diwu0510/p/11562248.html



#### 什么情况下需要引入第三方组件
+ 基于名称注入
+ 属性注入
+ 子容器
+ 基于动态代理的AOP

#### 组件包
+ Autofac.Extensions.DependencyInjection
+ Autofac.Extras.DynamicProxy(实现AOP的扩展包)

#### 案例总结
###### 拦截器
+ 配置允许在Controller类上使用拦截器并且允许属性注入
+ 配置允许在Interface中使用拦截器
+  拦截器在Class/Interface 中如何使用?(直接 ```[Intercept(typeof(MyInterceptor))]```)
+ 一个接口多个实现 为不同的实现指定名称在控制器中如何获取IContainer（需要自己实现一个类继承```Autofac.Module```并重写```Load()```方法）
+ 拦截器注册要在使用拦截器的接口和类型之前
+ 拦截器在Class中使用，仅virtual方法可以触发拦截器