[<img src="https://devforum.okta.com/uploads/oktadev/original/1X/bf54a16b5fda189e4ad2706fb57cbb7a1e5b8deb.png" align="right" width="256px"/>](https://devforum.okta.com/)

# Okta Xamarin SDK

> :warning: Beta alert! This library is in beta. See [release status](#release-status) for more information.

This library is a Xamarin library for communicating with Okta as an OAuth 2.0 + OpenID Connect provider, it follows current best practice for native apps using [Authorization Code Flow + PKCE](https://developer.okta.com/authentication-guide/implementing-authentication/auth-code-pkce).

## Release Status

This library uses semantic versioning and follows Okta's [library version policy](https://developer.okta.com/code/library-versions/).

| Version | Status                    |
| ------- | ------------------------- |
| 1.x | :warning: Beta |

The latest release is found on the [releases page](https://github.com/okta/okta-oidc-xamarin/releases).

## Usage Guide

To use the Okta Xamarin Sdk do the following:

1. Create an Okta account, also know as an _organization_, see [Developer Signup](https://developer.okta.com/signup/).
2. In your `Okta Developer Console` add an application; follow the directions at [Set up your Application](https://developer.okta.com/docs/guides/implement-auth-code-pkce/setup-app/) and accept the defaults.
3. In your `Okta Developer Console` register your application's login and logout redirect callbacks, see [Register Redirects](#register-redirects).
4. Configure your application to use the values registered in the previous step, see [Configure Your Application](#configure-your-application).
5. Add platform specific code, see [Platform Wiring](#platform-wiring).
6. Call `OktaContext.Current.SignIn` to begin the login flow.

### Register Redirects

To register redirect URIs do the following:

1. Sign in to your `Okta Developer Console` as an administrator.
2. Click the `Applications` tab and select your application.  If you need to set up your application see [Set up your Application](https://developer.okta.com/docs/guides/implement-auth-code-pkce/setup-app/). 
3. Ensure you are on the `General` tab, then go to `General Settings` and click `Edit`.
4. Go to the `Login` section.
5. Below `Login redirect URIs` click the `Add URI` button.
6. Enter a value appropriate for your application, this example uses the following:
    ```
    my.app.login:/callback
    ```
7. Below `Logout redirect URIs` click the `Add URI` button.
8. Enter a value appropriate for your application, this example uses the following:
    ```
    my.app.logout:/callback
    ```
9. Click `Save`.

## Configure Your Application

This section details how to configure your Okta Xamarin application.  These instructions assume you are using `Visual Studio` and were tested with `Visual Studio Community 2019` Version 16.8.0.

### Android Configuration

To configure your Android application do the following:

1. In the `Assets` folder of your Xamarin Android project, create a file called `OktaConfig.xml`.
2. Add the following content to the `OktaConfig.xml` file:
    ```xml
    <?xml version="1.0" encoding="utf-8" ?>
    <Okta>
        <ClientId>{ClientId}</ClientId>
        <OktaDomain>https://{yourOktaDomain}</OktaDomain>
        <RedirectUri>my.app.login:/callback</RedirectUri>
        <PostLogoutRedirectUri>my.app.logout:/callback</PostLogoutRedirectUri>
    </Okta>
    ```
    > Note: 
    > - The value entered for RedirectURI **MUST** match the value entered in step 6 of [Register Redirects](#register-redirects).
    > - The value entered for PostLogoutRedirectUri **MUST** match the value entere in step 8 of [Register Redirects](#register-redirects).
3. Replace `{ClientId}` and `{yourOktaDomain}` with appropriate values for your application, see [Find your Application's credentials](https://developer.okta.com/docs/guides/find-your-app-credentials/findcreds/).
4. Select the `OktaConfig.xml` file, then go to `View` -> `Properties Window`.
5. In the Properties window set the following value:
  - Build Action &mdash; `AndroidAsset`

### iOS Configuration

To configure your iOS application do the following:

1. In your Xamarin iOS project, double click the `Info.plist` file to open Visual Studio's Info.plist file editor.
2. Click the `Advanced` tab.
3. Expand the `URL Types` section.
4. Click the `Add URL Type` button.
5. In the `Identifier` field, enter the following:
    ```
    Okta OAuth login callback
    ```
6. In the `URL Schemes` field, enter a value appropriate for your application.  This example uses:
    ```
    my.app.login
    ```
    > Note: the value entered here **MUST** match the **prefix** entered in step 6 of [Register Redirects](#register-redirects).
7. In the `Role` dropdown select `Viewer`.
8. Click the `Add URL Type` button again.
9. In the `Identifier` field, enter the following:
    ```
    Okta OAuth logout callback
    ```
10. In the `URL Schemes` field, enter a value appropriate for your application.  This example uses:
    ```
    my.app.logout
    ```
    > Note: the value entered here **MUST** match the **prefix** entered in step 8 of [Register Redirects](#register-redirects).
11. In the `Role` dropdown select `Viewer`.
12. Save your changes.
13. In the root of your Xamarin iOS project, create a file called `OktaConfig.plist`.
14. Select the `OktaConfig.plist` file, then go to `View` -> `Properties Window`.
15. In the Properties window set the following values:
   - Build Action &mdash; `Content`
   - Copy to Output Directory &mdash; `Copy always`
16. Right click the `OktaConfig.plist` file and select `View Code`.
17. Replace the contents of the `OktaConfig.plist` file with the following:
    ```xml
    <?xml version="1.0" encoding="UTF-8"?>
    <!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
    <plist version="1.0">
    <dict>
        <key>ClientId</key>
        <string>{ClientId}</string>
        <key>OktaDomain</key>
        <string>https://{yourOktaDomain}</string>
        <key>RedirectUri</key>
        <string>my.app.login:/callback</string>
        <key>PostLogoutRedirectUri</key>
        <string>my.app.logout:/callback</string>
    </dict>
    </plist>
    ```
    > Note: 
    > - The value entered for RedirectURI **MUST** match the value entered in step 6 of [Register Redirects](#register-redirects).
    > - The value entered for PostLogoutRedirectUri **MUST** match the value entered in step 8 of [Register Redirects](#register-redirects).
18. Replace `{ClientId}` and `{yourOktaDomain}` with appropriate values for your application, see [Find your Application's credentials](https://developer.okta.com/docs/guides/find-your-app-credentials/findcreds/).


## Platform Wiring

This section describes the minimal code necessary to handle Okta authentication related redirects when using the Okta Xamarin Sdk.  The examples shown here are based on `Xamarin.Forms` projects.

### Android

To handle Okta authentication redirects on Android do the following:

1. In the `OnCreate` method of your `MainActivity` class initialize the `OktaContext` with the following code:
    ```csharp
    OktaContext.Init(new OidcClient(this, OktaConfig.LoadFromXmlStream(Assets.Open("OktaConfig.xml"))));
    ```
    > Ensure that your OktaContext calls are made prior to `base.OnCreate`.
2. Additionally, in the `OnCreate` method of your `MainActivity` class add event handlers for the `SignInCompleted` and `SignOutCompleted` events, this example navigates to the `ProfilePage`, you should provide logic appropriate for your application:
    ```csharp
    OktaContext.AddSignInCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
    OktaContext.AddSignOutCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
    ```
    > Ensure that your OktaContext calls are made prior to `base.OnCreate`.  
    > A complete example of a `MainActivity` class follows:
    ```csharp    
    [Activity(Label = "MainActivity", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            OktaContext.Init(new OidcClient(this, OktaConfig.LoadFromXmlStream(Assets.Open("OktaConfig.xml"))));
            OktaContext.AddSignInCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
            OktaContext.AddSignOutCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            global::Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] global::Android.Content.PM.Permission[] grantResults)
        {
            global::Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
    ```
3. Create a new Activity to intercept Login redirects, this example uses `MyLoginCallbackInterceptorActivity`.
4. Replace the activity implementation with the following code:
    ```csharp
    [Activity(Label = "MyLoginCallbackInterceptorActivity")]
    [IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
            DataSchemes = new[] { "my.app.login" },
            DataPath = "/callback"
        )
    ]
    public class MyLoginCallbackInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Android.Net.Uri uri_android = Intent.Data;

            if (global::Okta.Xamarin.OidcClient.InterceptLoginCallback(new Uri(uri_android.ToString())))
            {
                this.Finish();
            }

            return;
        }
    }
    ```
    > Note that the value specified for `DataSchemes` **MUST** match the prefix entered in step 6 of [Register Redirects](#register-redirects) and the `DataPath` **MUST** match the suffix.
5. Create a new Activity to intercept Logout redirects, this example uses `MyLogoutCallbackInterceptorActivity`.
6. Replace the activity implementation with the following code:
    ```csharp
    [Activity(Label = "MyLogoutCallbackInterceptor")]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
            DataSchemes = new[] { "my.app.logout" },
            DataPath = "/callback"
        )
    ]
    public class MyLogoutCallbackInterceptor : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            global::Android.Net.Uri uri_android = Intent.Data;

            if (global::Okta.Xamarin.OidcClient.InterceptLogoutCallback(new Uri(uri_android.ToString())))
            {
                this.Finish();
            }

            return;
        }
    }
    ```
     > Note that the value specified for `DataSchemes` **MUST** match the prefix entered in step 8 of [Register Redirects](#register-redirects) and the `DataPath` **MUST** match the suffix.

### iOS

To handle Okta authentication redirects on iOS do the following:

1. Modify your `AppDelegate` class to extend `OktaAppDelegate<App>` (or `OktaAppDelegate` if you are not using Xamarin.Forms).
2. In the `FinishedLaunching` method add event handlers for the `SignInCompleted` and `SignOutCompleted` events, this example navigates to the `ProfilePage`, you should provide logic appropriate for your application:
    ```csharp
    OktaContext.AddSignInCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
    OktaContext.AddSignOutCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
    ```
    > A complete AppDelegate example follows:
    ```csharp
    [Register("AppDelegate")]
    public partial class AppDelegate : OktaAppDelegate<App>
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            bool result = base.FinishedLaunching(app, options);
            OktaContext.AddSignInCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));
            OktaContext.AddSignOutCompletedListener((sender, args) => Shell.Current.GoToAsync("//ProfilePage"));

            return result;
        }
    }
    ```

## API Reference

### OktaContext.Current

The `OktaContext.Current` singleton provides a top level entry point into Okta functionality.  To begin the login flow use the `SignIn` method:
```csharp
OktaContext.Current.SignIn();
```

Similarly, use the `SignOut` method to log a user out:
```csharp
OktaContext.Current.SignOut();
```

### SignInCompleted event

The `OktaContext.Current.SignInCompleted` event is raised when the login flow completes.  To execute code when the `SignInCompleted` event is raised, add an event handler to the `OktaContext.Current.SignInCompleted` event.  This is done directly or using the static `AddSignInCompletedListener` method.

```csharp
// directly
OktaContext.Current.SignInCompleted += (sender, args) => Console.WriteLine("SignIn completed");

// using AddSignInCompletedListener
OktaContext.AddSignInCompletedListener((sender, args) => Console.WriteLine("SignIn completed"));
```

### SignOutCompleted event
The `OktaContext.Current.SignOutCompleted` event is raised when the logout flow completes.  To execute code when the `SignOutCompleted` event is raised, add an event handler to the `OktaContext.Current.SignOutCompleted` event.  This is done directly or using the static `AddSignOutCompletedListener` method.

```csharp
// directly
OktaContext.Current.SignOutCompleted += (sender, args) => Console.WriteLine("SignOut completed");

// using AddSignOutCompletedListener
OktaContext.AddSignOutCompletedListener((sender, args) => Console.WriteLine("SignOut completed"));
```

### OktaState, BearerToken and BearerTokenClaims classes

When the `SignInCompleted` event is raised the `EventArgs` parameter instance is of type `SignInEventArgs` which provides access to an instance of the `OktaState` class.  Use code similar to the following to read the authentication state and access the bearer token claims when sign in completes:

```csharp
OktaContext.AddSignInCompletedListener((sender, args) =>
{
    SignInEventArgs signInEventArgs = (SignInEventArgs)args;
    OktaState oktaState = signInEventArgs.StateManager; //
    BearerToken bearerToken = new BearerToken(oktaState.AccessToken);
    BearerTokenClaims claims = BearerTokenClaims.FromBearerToken(bearerToken);

    // Access claims properties
    Console.WriteLine(claims.Issuer);
    Console.WriteLine(claims.Subject);
    Console.WriteLine(claims.Audience);
    Console.WriteLine(claims.ExpirationTime);
});
```

## Contributing

We're happy to accept contributions and PRs! Please see the [contribution guide](CONTRIBUTING.md) to understand how to structure a contribution.

## Need help?
 
If you run into problems using the SDK, you can
 
* Ask questions on the [Okta Developer Forums](https://devforum.okta.com/)
* Post [issues](https://github.com/okta/okta-oidc-xamarin/issues) here on GitHub (for code errors)