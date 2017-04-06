# NavigationLib
A simple navigation sub system for WinForms projects.

Super simple to register views.

```C#
NavigationSystem.Register("HomePage", new HomeView());
NavigationSystem.Register("SettingsPage", new SettingsView());
```

Set a home view which may be visited frequently
```C#
NavigationSystem.SetHome("HomePage");
NavigationSystem.GoHome();
```

Navigate using keys.
```C#
NavigationSystem.NavigateTo("SettingsPage");
NavigationSystem.GoBack();
```

## Message System
The navigation library also provides a basic messaging system to communicate data between views.

```C#
MessageSystem.SendMessage("SettingsPage", "EnableField", true);
```

You can also broadcast a message to all views.

```C#
MessageSystem.Broadcast("PopulateBuffers", { "Any Data", "Any Type" });
```
