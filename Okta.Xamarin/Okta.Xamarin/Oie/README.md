
# Xamarin Okta Identity Engine Client SDK

This namespace contains the `Xamarin Okta Identity Engine` client implementation.  The `Xamarin Okta Identity Engine` or `XOIE` allows you to use `Okta Identity Engine` to authenticate users to your application using a customizable authentication pipeline.

## Initialization

To initialize `XOIE` pass an `OktaOptions` argument to the `OktaPlatform.Init` method with `FlowMode` set to `FlowModes.Oie`.

### Android

To initailize `XOIE` on Android add the following code to the `OnCreate` method of your `MainActivity`:

```csharp
OktaPlatform.Init(new OktaOptions
{
    ApplicationContext = this,
    FlowMode = FlowModes.Oie
}); 
```

### iOS

To initialize `XOIE` on iOS add the following code to the `FinishedLaunching` method of your `AppDelegate`:

```csharp
OktaPlatform.Init(new OktaOptions
{
    ViewController = Window.RootViewController,
    FlowMode = FlowModes.Oie
});
```

Additionally, add the following code to the `OpenUrl` method of your `AppDelegate`:

```csharp
OktaService.OpenUrl(application, url, sourceApplication, annotation);
```

## Customized UI

`XOIE` supports custom themes using Okta's brands API, see [Brands API](https://developer.okta.com/docs/reference/api/brands/).  To use a custom theme, specify brand and theme IDs when the Okta plaform is initialized.

### Android

```csharp
OktaPlatform.Init(new OktaOptions
{
    ApplicationContext = this,
    FlowMode = FlowModes.Oie,
    BrandId = "brand1234",
    ThemeId = "theme5678"
});
```

### iOS

```csharp
OktaPlatform.Init(new OktaOptions
{
    ViewController = Window.RootViewController,
    FlowMode = FlowModes.Oie,
    BrandId = "brand1234",
    ThemeId = "theme5678"
})
```

