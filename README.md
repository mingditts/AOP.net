# AOP

AOP is a library for use the Aspect Oriented approach in a .net project.
It's work in progress.

Usage:
```
/// <summary>
/// Your aspect
/// </summary>
/// <typeparam name="T"></typeparam>
private class MyAspect<T> : Aspect<T>, IBeforeAdvice, IAroundAdvice, IAfterAdvice, IAfterThrowAdvice where T : class
{
	public bool OnBeforeReached = false;
	public bool OnAroundReached = false;
	public bool OnAfterReached = false;
	public bool OnThrowReached = false;

	private bool _proceed = true;
	private object _overwrittenResult = null;

	public MyAspect()
	{

	}

	public MyAspect(bool proceed, object overwrittenResult)
	{
		this._proceed = proceed;
		this._overwrittenResult = overwrittenResult;
	}

	public void OnBefore(ExecutionContext context)
	{
		this.OnBeforeReached = true;
	}

	public AroundExecutionResult OnAround(ExecutionContext context)
	{
		this.OnAroundReached = true;
		return new AroundExecutionResult
		{
			Proceed = this._proceed,
			OverwrittenResult = this._overwrittenResult
		};
	}

	public void OnAfter(ExecutionContext context, object result)
	{
		this.OnAfterReached = true;
	}

	public void OnThrow(ExecutionContext context, Exception exception)
	{
		this.OnThrowReached = true;
	}
}

//IService and Service are your object/service class to observe/control

var aspect = new MyAspect<IService>();

IService service = aspect.Build(new Service());

service.DoWork();
```

### Features

  - It manages Before, Around (with execution control), After and Throw pointcuts
  - It allows the aspect waterfall (chain) in order to register different aspects on the same object

### Todos

  - Test coverage
  - Manage task execution

### License

MIT

THIS SOFTWARE IS PROVIDED "AS IS" BY THE OWNER AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE OWNER BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
