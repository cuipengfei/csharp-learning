50

use analyzers

# 例如在当前项目中enable如下配置

```
<PropertyGroup>
    <EnableNETAnalyzers>true</EnableNETAnalyzers>
    <AnalysisMode>AllEnabledByDefault</AnalysisMode>
    <AnalysisLevel>latest</AnalysisLevel>
</PropertyGroup>
```

下一次dotnet build时将出现更多的warning

# 其他可选analyzer

https://www.meziantou.net/the-roslyn-analyzers-i-use.htm

https://github.com/dotnet/roslyn-analyzers

http://code-cracker.github.io/
