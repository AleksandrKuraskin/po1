using ConsoleRpg.Shared.Core;

namespace ConsoleRpg.Client.View;

public class ViewObserver(IClientModel model) : IStateObserver
{
    private readonly IClientModel _model = model;
    private readonly object _renderLock = new object();

    public void Update()
    {
        lock (_renderLock)
        {
            _model.Renderer.Render(_model);
        }
    }
}
