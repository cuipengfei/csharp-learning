using System;

// 尽量采用隐式属性来表示可变的数据
public class Test {
    
    public string Test1 {get;set;}
    
        
    private string _test2;

    public string Test2 
    {
        get => _test2;
        set 
        {
            _test2 = value;
        }
    }
}


[Serializable]
public class TestSerializable {
    
    public string Test3 {get;set;}
}