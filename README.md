# LightHook-Core
A lightweight hooking framework powered by .NET NativeAOT. Enables dependency-free API interception and memory operations through JavaScript-style scripting. 2MB core with future cross-platform roadmap.  
本项目受Frida启发，是一个基于 .NET NativeAOT 构建的高性能动态插桩框架，通过类 JavaScript 的脚本注入实现 API Hook 和内存操作，无外部依赖，2MB 轻量级核心，未来将扩展跨平台支持。

---

本项目采用的脚本引擎：[xy660/TinyScript](https://github.com/xy660/TinyScript) 有关基础语法和与JavaScript的差异可以参阅其仓库内的文档。

## 快速示例

Hook系统API:

```javascript
ffiload("kernel32.dll","GetLastError","","int");        //load the system api
ffiload("kernel32.dll","SetLastError","int","void");
ffiload("kernel32.dll", "CreateProcessW","string,string,ptr,ptr,bool,int,ptr,string,ptr,ptr","bool");

var pfn = getProcAddress("kernel32.dll", "CreateProcessW");       //get the function pointer

var conv = "stdcall"; // Adapt platform ABI
if(is64bit()) conv = "win64";

var cb = createCallback({
    func: function (app, cmd, pa, ta, ih, fl, env, cd, si, pi) {
        println("app=" + app); //Print to remote console
        println("cmd=" + cmd);
        println("pa=" + pa);
        println("ta=" + ta);
        println("ih=" + ih);
        println("fl=" + fl);
        println("env=" + env);
        println("cd=" + cd);
        println("si=" + si);
        println("pi=" + pi);

        suspend("cpwhook"); //suspend the hook
        var ret = CreateProcessW(app, cmd, pa, ta, ih, fl, env, cd, si, pi); //call original function
        resume("cpwhook"); //resume the hook

        if(!ret) return false;
        
        var pid = 0; 
        if(is64bit()){   //get pid using the struct member offset
            var p = pi.copy();
            pid = p.move(16).readInt();
        }else{
            var p = pi.copy();
            pid = p.move(8).readInt();
        }
        println("pid="+pid);
        inject(pid); //inject subprocess
        return true;
    },
    argTypes: "string,string,ptr,ptr,bool,int,ptr,string,ptr,ptr",
    retType: "bool",
    abi: conv
});

hook(pfn, cb, "cpwhook"); //install hook

println("install hook successful");

```

效果图：

![效果图](https://raw.githubusercontent.com/xy660/LightHook-Core/main/imgs/1.png)

---

## 未来开发计划：

- Console命令行注入器代替Winforms
- Linux平台支持
- Arm架构支持


