# NavigationLib
A simple navigation sub system for WinForms projects.

`
NavigationSystem.Register("HomePage", new HomeView());
`

`
NavigationSystem.Register("SettingsPage", new SettingsView());
`

`
NavigationSystem.SetHome("HomePage");
`

`
NavigationSystem.NavigateTo("SettingsPage");
`

## Message System
The navigation library also provides a basic messaging system to communicate data between views.

`
MessageSystem.SendMessage("SettingsPage", "EnableField", true);
`

You can also broadcast a message to all views.

`
MessageSystem.Broadcast("PopulateBuffers", { "Any Data", "Any Type" });
`
