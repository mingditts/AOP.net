# AOP

AOP is a library for use the Aspect Oriented approach in a .net project.

Usage:
```
/// <summary>
/// Your aspect
/// </summary>
/// <typeparam name="T"></typeparam>
private class MyAspect<T> : Aspect<T>, IBeforeAdvice, IAroundAdvice, IAfterAdvice, IAfterThrowAdvice where T : class
{
	public MyAspect()
	{

	}

	public void OnBefore(ExecutionContext context)
	{
		//code executed before
	}

	public AroundExecutionResult OnAround(ExecutionContext context)
	{
		//code executed around
		
		return new AroundExecutionResult
		{
			Proceed = true,
			OverwrittenResult = null
		};
	}

	public void OnAfter(ExecutionContext context, object result)
	{
		//code executed after
	}

	public void OnThrow(ExecutionContext context, Exception exception)
	{
		//code executed on exception throw
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
