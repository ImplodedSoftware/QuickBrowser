using System;
using ImGeneralPluginEngine;
using NeonScripting;

namespace QuickBrowse
{
    public class QuickBrowsePlugin : IImGeneralPlugin
    {
        private QuickBrowseWindow _quickBrowseWindow;
        public void InitializePlugin(INeonScriptHost host, Action<IImGeneralPlugin> pluginCloseAction)
        {
            PluginHostHandler.Instance.ScriptHost = host;
            PluginHostHandler.Instance.PluginCloseAction = pluginCloseAction;
            PluginHostHandler.Instance.ThisPlugin = this;
            _quickBrowseWindow = new QuickBrowseWindow();
            _quickBrowseWindow.Show();
        }

        public void OnEvent(NeonEventTypes eventType)
        {
            // not used
        }

        public void ClosePlugin()
        {
            _quickBrowseWindow.Close();
        }

        public string Name => "Quick browse";
        public string Author => "Mikael Stalvik";
    }
}
