[<img src="https://devforum.okta.com/uploads/oktadev/original/1X/bf54a16b5fda189e4ad2706fb57cbb7a1e5b8deb.png" align="right" width="256px"/>](https://devforum.okta.com/)

# Okta Xamarin SDK

> :warning: Beta alert! This library is in beta. See [release status](#release-status) for more information.

This library is a Xamarin library for communicating with Okta as an OAuth 2.0 + OpenID Connect provider, and follows current best practice for native apps using [Authorization Code Flow + PKCE](https://developer.okta.com/authentication-guide/implementing-authentication/auth-code-pkce).

You can learn more on the [Okta + .NET](https://developer.okta.com/code/xamarin/) page in our documentation.

**Table of Contents**
- [Release status](#release-status)
- [Need help?](#need-help)
- [Getting Started](#getting-started)
- [Usage Guide](#usage-guide)
- [Configuration Reference](#configuration-reference)
  - [Okta.json](#oktajson)
  - [Configuration object](#configuration-object)
  - [Validating the config](#validating-the-config)
- [Register a callback](#registering-callbacks)
  - [Android](#android)
  - [iOS](#ios)
- [API Reference](#api-reference)
  - [OidcClient](#oidcclient)
    - [SignInWithBrowserAsync()](#signinwithbrowserasync)
- [Contributing](#contributing)


## Release Status

This library uses semantic versioning and follows Okta's [library version policy](https://developer.okta.com/code/library-versions/).

| Version | Status                    |
| ------- | ------------------------- |
| 1.x | :warning: Beta |

The latest release can always be found on the [releases page](https://github.com/okta/okta-oidc-xamarin/releases).

## Need help?
 
If you run into problems using the SDK, you can
 
* Ask questions on the [Okta Developer Forums](https://devforum.okta.com/)
* Post [issues](https://github.com/okta/okta-oidc-xamarin/issues) here on GitHub (for code errors)

## Getting Started 

The Okta.Xamarin SDK is compatible with [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard).

Installing the Okta.Xamarin SDK into your project is simple. The easiest way to include this library into your project is through NuGet.

### Android

```cmd
Install-Package Okta.Xamarin.Android
```

### iOS

```cmd
Install-Package Okta.Xamarin.iOS
```

You'll also need:

- An Okta account, called an _organization_ (sign up for a free [developer organization](https://developer.okta.com/signup/) if you need one).
- An Okta Application, configured as a Native App. This is done from the Okta Developer Console and you can find instructions [here](https://developer.okta.com/authentication-guide/implementing-authentication/auth-code-pkce). When following the wizard, use the default properties. They are designed to work with our sample applications.


## Usage Guide

1. [Register a callback](#registering-callbacks) (via ActivityCustomUrlSchemeInterceptor on Android or CFBundleURLTypes and AppDelegate on iOS) to allow the browser to call back to your app after login.
2. [Create an `OktaConfig`](#configuration-reference) by loading a configuration file or specifying configuration details in code.
3. [Create an `OidcClient`](#oidcclient), passing in `this` (a reference to the current Android Activity or iOS ViewController) and the config object
4. When a user wants to sign in, [call `SignInWithBrowserAsync`](#signinwithbrowserasync) on the OidcClient.  This is an async function and must be awaited.
5. Use the returned [`StateManager`](#statemanager) to get the access token and other login details.

## Configuration Reference

The entry point for the SDK is an instance of `Okta.Xamarin.OidcClient`.  When you instantiate an `OidcClient`, you need to include a configuration object by passing in a `Okta.Xamarin.OktaConfig`. 

```csharp
// Load configuration from a json file
var config = await Okta.Xamarin.OktaConfig.LoadFromJsonFileAsync(/* path to .json file */)

// Load configuration from an xml asset on Android
var config = await Okta.Xamarin.OktaConfig.LoadFromXmlStreamAsync(Assets.Open("OktaConfig.xml")))

// Load configuration from a plist file on iOS
var config = Okta.Xamarin.OktaConfig.LoadFromPList(/* plist file name */)

// Specify config manually
var config = new Okta.Xamarin.OktaConfig() {
    OktaDomain = "https://{yourOktaDomain}",
    ClientId = "{clientId}",
    RedirectUri = "{redirectUri}",
    PostLogoutRedirectUri = "{postLogoutRedirectUri}",
    Scope = "openid profile offline_access"
};

// Instantiate Okta.Xamarin.OidcClient with a configuration object
var oidcClient = new Okta.Xamarin.OidcClient(this, config)

```

### Configuration file

The easiest way to load configuration is to load a json file.  Additionally on Android you can use an xml file from your `Assets` directory, and on iOS you can use a plist.

If loading an xml config from an Android Assets directory, the file's *build action* should be set to `AndroidAsset`.  Otherwise, set any config file as *build action*: `Content` and *copy to output directory*: `Copy always` or `Copy if newer`.

Here is an example json file that will work in both Android and iOS:
```json
{
    "OktaDomain": "https://{yourOktaDomain}",
    "ClientId": "{clientId}",
    "RedirectUri": "{yourOktaScheme}:/callback",
    "PostLogoutRedirectUri": "{yourOktaScheme}:/logout",
    "Scope": "openid profile offline_access"
}
```

Here is the equivalent Android xml file:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<Okta>
    <OktaDomain>https://{yourOktaDomain}</OktaDomain>
    <ClientId>{clientId}<ClientId>
    <RedirectUri>{yourOktaScheme}:/callback</RedirectUri>
    <PostLogoutRedirectUri>{yourOktaScheme}:/logout</PostLogoutRedirectUri>
    <Scope>openid profile offline_access</Scope>
</Okta>
```

And the equivalent iOS plist file:
```xml
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>ClientId</key>
    <string>{clientId}</string>
    <key>OktaDomain</key>
    <string>https://{yourOktaDomain}</string>
    <key>RedirectUri</key>
    <string>{yourOktaScheme}:/callback</string>
    <key>PostLogoutRedirectUri</key>
    <string>{yourOktaScheme}:/logout</string>
    <key>Scope</key>
    <string>openid profile offline_access</string>
</dict>
</plist>

```


### Configuration object

Alternatively, you can create a configuration object in code ( `Okta.Xamarin.OktaConfig`) with the required values:

```csharp
var config = new Okta.Xamarin.OktaConfig() {
    OktaDomain = "https://{yourOktaDomain}",
    ClientId = "{clientId}",
    RedirectUri = "{yourOktaScheme}:/callback",
    PostLogoutRedirectUri = "{yourOktaScheme}:/logout",
    Scope = "openid profile offline_access"
}
```

### Validating the config

You can use an OktaConfigValidator to ensure that a given configuration object is valid.  This is called automatically when using a config to instantiate an OidcClient, but in case you want to call it manually, here's how:

```csharp
OktaConfigValidator<OktaConfig> validator = new OktaConfigValidator<OktaConfig>();

validator.Validate(myConfigObject);
// throws an exception if the config is invalid
```

## Registering Callbacks

The browser-based login is securely implemented on Android with Chrome Custom Tabs and on iOS through Safari ViewController.  In order to complete the flow, the browser must be able to redirect back to the application, which then needs to process the response.  You need to do a bit of work to hook all this up.

In the following examples, assume we have a `RedirectUri` in our config as well as on the Okta application dashboard set to `com.myappnamespace.exampleapp:/callback`

### Android

Create a new Activity in your app.  Edit the code for your new activity to make it inherit from `Okta.Xamarin.Android.ActivityCustomUrlSchemeInterceptor` rather than Activity or AppCompatActivity.  On this class, you need to set the Activity attributes to `NoHistory = true, LaunchMode = LaunchMode.SingleTop` and also set the IntentFilter attribute to include the `DataSchemes` that matches the scheme of your RedirectUri (the part before the ":/", where "https" would normally go in a url) and `DataPath` that matches the path of your RedirectUri.

Here is an example activity called `ExampleActivityCustomUrlSchemeInterceptor`:
```csharp
[Activity(Label = "ExampleActivityCustomUrlSchemeInterceptor", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
[IntentFilter(actions: new[] { Intent.ActionView },
    Categories = new[] {Intent.CategoryDefault,Intent.CategoryBrowsable},
    DataSchemes = new[] {"com.myappnamespace.exampleapp"},
    DataPath = "/callback" )]
public class ExampleActivityCustomUrlSchemeInterceptor : ActivityCustomUrlSchemeInterceptor
{ }
```

### iOS

Modify your `Info.plist` to include a `CFBundleURLSchemes` under `CFBundleURLTypes`, which matches the scheme of your RedirectUri.

Here is an example Info.plist:
```xml
<key>CFBundleURLTypes</key>
<array>
    <dict>
        <key>CFBundleURLName</key>
        <string>MyExample OAuth</string>
        <key>CFBundleURLSchemes</key>
        <array>
            <string>com.myappnamespace.exampleapp</string>
        </array>
        <key>CFBundleURLTypes</key>
        <string>Viewer</string>
    </dict>
</array>
```

In your `AppDelegate.cs`, override `OpenUrl` and pass its arguments to the static function `Okta.Xamarin.OidcClient.OpenUrl`.  This  returns `false` if it is not given a valid login RedirectUrl, so if you are deep linking other urls into your app for other functionality you can continue processing in that case.

Here is an example OpenUrl to add to your AppDelegate.cs:
```csharp
public override bool OpenUrl(UIApplication application,NSUrl url,string sourceApplication,NSObject annotation)
{
    return Okta.Xamarin.OidcClient.OpenUrl(application, url, sourceApplication, annotation);
}
```


## API Reference

### OidcClient

The `Okta.Xamarin.OidcClient` class contains methods for signing in, signing out, and authenticating sessions.

#### SignInWithBrowserAsync()

Start the authorization flow by simply calling `SignInWithBrowserAsync()`.  This is an async method and should be awaited.  In case of successful authorization, this operation will return valid `Okta.Xamarin.StateManager` object.  Clients are responsible for further storage and maintenance of the manager.

```csharp
StateManager stateManager = await oidcClient.SignInWithBrowserAsync();

// stateManager.IsAuthenticated;
// stateManager.AccessToken
// stateManager.IdToken
// stateManager.RefreshToken
```

## Contributing

We're happy to accept contributions and PRs! Please see the [contribution guide](CONTRIBUTING.md) to understand how to structure a contribution.