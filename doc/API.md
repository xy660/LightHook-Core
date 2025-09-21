# LightHook-Core 脚本API文档

## Hook安装与卸载

- **hook(pSrc,pCallback,name)**

参数说明：

1. pSrc : PTR        要hook的函数地址
2. pCallback : PTR   你的回调地址
3. name : STRING     唯一名称

返回值：无


- **unhook(name)**

参数说明：

1. name : STRING    唯一名称

返回值：无

- **createCallback(info)**

参数说明：

1. info : OBJECT    函数信息

info中包含的成员：
```javascript
{
  func: 你的回调方法
  argTypes: 函数参数签名
  retType: 返回值类型
  abi: 调用约定
}
```

关于此方法具体描述，请参阅[TinyScript FFI文档](https://github.com/xy660/TinyScript/blob/main/doc/FFI.md)

- storageSet(name,value)

参数说明：

1. name : STRING    唯一名称
2. value : ANY      要存储的值

返回值：无

此方法存储的值整个脚本运行环境内有效，适用于全局存储



- storageGet(name)

参数说明：

1. name : STRING      唯一名称

返回值：value : ANY    当前存储的值，如果找不到此值，则会抛出异常

---

有关更多语法/内置方法文档，参阅[TinyScript 文档](https://github.com/xy660/TinyScript/blob/main/doc)







