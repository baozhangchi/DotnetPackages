1. 安装
    ````bash
    dotnet add package baozhangchi.Packages.Windows
    ````
2. 导入命名空间
    ````xaml
    xmlns:dp="https://github.com/baozhangchi/dotnetpackages"
    ````
2. 说明  

    1. `Behaviors`：原生控件的附加属性，已添加到默认命名空间中
    2. `Converters`：常用的转换器，已添加到默认命名空间中
    
        1. `CommandParametersToTupleConverter`：`CommandParameters`转成`Tuple`的转换器，用于对`CommandParameters`的`MultiBinding`
    3. `Extensions`：常用的扩展方法
    4. `MarkupExtensions`：常用的标记扩展，已添加到默认命名空间中
    
        1. `EnumBindingSourceExtension`：把一个枚举类型转化成对应的数据源
    5. `TypeConverters`：常用的类型转换
    6. `Command`：`ICommand`的一个实现
    9. `PropertyChangedBase`： 支持`INotifyPropertyChanging`和`INotifyPropertyChanged`接口的基类