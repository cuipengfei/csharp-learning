## 尽早执行 和延迟执行


```c#
var answer = DoSuff(method1(), method2(), method3())


var answer = DoSuff(() => method1(), () => method2(), () => method3())
```