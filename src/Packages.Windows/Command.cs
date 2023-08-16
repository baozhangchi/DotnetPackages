// ReSharper disable once CheckNamespace

namespace System.Windows.Input;

/// <inheritdoc />
public abstract class CommandBase : ICommand
{
    #region Fields

    private readonly Func<object?, bool> _canExecute;
    private readonly Action<object?> _execute;

    #endregion

    #region Constructors

    /// <summary>
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    /// <exception cref="ArgumentNullException"></exception>
    protected CommandBase(Action<object?> execute, Func<object?, bool> canExecute)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute ?? throw new ArgumentNullException(nameof(canExecute));
    }

    #endregion

    #region Methods

    /// <summary>
    ///     触发CanExecuteChanged
    /// </summary>
    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    public bool CanExecute(object? parameter)
    {
        return _canExecute(parameter);
    }

    /// <inheritdoc />
    public void Execute(object? parameter)
    {
        if (CanExecute(parameter))
        {
            _execute(parameter);
        }
    }

    /// <inheritdoc />
    public event EventHandler? CanExecuteChanged;

    #endregion
}

/// <summary>
///     ICommand不带参数实现
/// </summary>
public class Command : CommandBase
{
    #region Constructors

    /// <summary>
    ///     初始化Command
    /// </summary>
    /// <param name="execute"></param>
    public Command(Action execute) : this(execute, () => true)
    {
    }

    /// <summary>
    ///     初始化Command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public Command(Action execute, Func<bool> canExecute) : base(_ => execute(), _ => canExecute())
    {
    }

    #endregion
}

/// <summary>
///     ICommand带参数实现
/// </summary>
/// <typeparam name="T">参数类型</typeparam>
public class Command<T> : CommandBase
{
    #region Constructors

    /// <summary>
    ///     初始化Command
    /// </summary>
    /// <param name="execute"></param>
    /// <param name="canExecute"></param>
    public Command(Action<T?> execute, Func<T?, bool> canExecute) : base(o => execute(o == null ? default : (T)o), o => canExecute(o == null ? default : (T)o))
    {
    }

    /// <summary>
    ///     初始化Command
    /// </summary>
    /// <param name="execute"></param>
    public Command(Action<T?> execute) : this(execute, _ => true)
    {
    }

    #endregion
}