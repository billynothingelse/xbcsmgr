using System;
using System.Windows;
using System.Windows.Controls.Primitives;

namespace XboxCsMgr.Client.Controls.Custom
{
	public class XCsTreeViewToggle : ToggleButton
	{
		public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
			"IsActive", typeof(Boolean), typeof(XCsTreeViewToggle), new PropertyMetadata(default(Boolean)));

		public Boolean IsActive
		{
			get { return (Boolean)GetValue(IsActiveProperty); }
			set { SetValue(IsActiveProperty, value); }
		}
	}
}
