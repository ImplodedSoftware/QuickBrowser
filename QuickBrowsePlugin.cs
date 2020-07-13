using System;
using ImGeneralPluginEngine;
using ImGeneralPluginEngine.Abstractions;
using NeonScripting;
using NeonScripting.Models;

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

        public void OnEvent(NeonScriptEventTypes eventType)
        {
            // not used
        }

        public void ClosePlugin()
        {
            _quickBrowseWindow.Close();
        }

        public string Name => "Quick browser";
        public string Author => "Mikael Stalvik";
    }
}
