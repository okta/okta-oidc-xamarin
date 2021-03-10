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

## Create New Xamarin.Forms Project

This section describes how to configure your Xamarin Forms application to use Okta Oidc.  These instructions assume you are using `Visual Studio` and were tested with `Visual Studio Community 2019` Version 16.8.0.  For the purposes of this example the project name used is `MyOktaApp`.

Create a new `Mobile App (Xamarin.Forms)` project:

1. Start Visual Studio and select "Create a new project".
2. Find and select the project template named `Mobile App (Xamarin.Forms)`, click next.
3. Enter `MyOktaApp` into the `Project name` field of the `Configure your new project` form.
4. Fill in the remaining fields of the `Configure your new project` form and click `Create`.

Now that your Visual Studio solution is initialized, you can continue to update package references.

## Update Package References Automatically

This section describes how to update package references automatically.  If you prefer to update package references manually, see [Update Package References Manually](#update-package-references-manually).

To update package references automatically do the following:

1. Go to `View` > `Other Windows` > `Package Manager Console`.
2. In the `Package Manager Console` window type `Update-Package` and press enter.

## Update Package References Manually

This section describes how to update package references manually.  If you prefer to update package references automatically, see [Update Package References Automatically](#update-package-references-automatically).

The following operations are performed from `Solution Explorer`; to ensure `Solution Explorer` is visible go to `View` > `Solution Explorer`.

To update package references do the following:

1. Right click `MyOktaApp` and select `Manage Nuget Packages`.
2. From the `Installed` tab select `Xamarin.Essentials` and click `Update`; this should update `Xamarin.Essentials` to the latest version (v1.6.1 as of 03/08/2021).
3. From the `Installed` tab select `Xamarin.Forms` and click `Update`; this should update `Xamarin.Forms` to the latest version (v5.0.0.2012 as of 03.08/2021).
4. Right click `MyOktaApp.Android` and select `Manage Nuget Packages`.
5. From the `Installed` tab select `Xamarin.Essentials` and click `Update`; this should update `Xamarin.Essentials` to the latest version (v1.6.1 as of 03/08/2021).
6. From the `Installed` tab select `Xamarin.Forms` and click `Update`; this should update `Xamarin.Forms` to the latest version (v5.0.0.2012 as of 03.08/2021).
7. Right click `MyOktaApp.iOS` and select `Manage Nuget Packages`.
2. From the `Installed` tab select `Xamarin.Essentials` and click `Update`; this should update `Xamarin.Essentials` to the latest version (v1.6.1 as of 03/08/2021).
3. From the `Installed` tab select `Xamarin.Forms` and click `Update`; this should update `Xamarin.Forms` to the latest version (v5.0.0.2012 as of 03.08/2021).

## Add Okta.Xamarin

This section describes how to add Xamarin related Okta packages to your projects.

The following operations are performed from `Solution Explorer`; to ensure `Solution Explorer` is visible go to `View` > `Solution Explorer`.

To add Okta Xamarin packages do the following:

1. Right click on the project `MyOktaApp` and select `Manage Nuget Packages...`.
2. In the `NuGet Package Manager` window click `Browse`.
3. In the search box type `Okta.Xamarin`.
4. Select `Okta.Xamarin` then click the `Install` button.  Accept defaults on any prompts that may appear.
5. Right click on the project `MyOktaApp.Android` and select `Manage Nuget Packages...`.
6. In the `NuGet Package Manager` window click `Browse`.
7. In the search box type `Okta.Xamarin`.
8. Select `Okta.Xamarin` then click the `Install` button.  Accept defaults on any prompts that may appear.
9. Select `Okta.Xamarin.Android` then click the `Install` button.  Accept defaults on any prompts that may appear.
10. Right click on the project `MyOktaApp.iOS` and select `Manage Nuget Packages...`.
11. In the `NuGet Package Manager` window click `Browse`.
12. In the search box type `Okta.Xamarin`.
13. Select `Okta.Xamarin` then click the `Install` button.  Accept defaults on any prompts that may appear.
14. Select `Okta.Xamarin.iOS` then click the `Install` button.  Accept defaults on any prompts that may appear.

## Update Android Target Framework

The minimum supported Android version of the `Okta Xamarin Sdk` is `Android 10.0 (Q)`.

The following operations are performed from `Solution Explorer`; to ensure `Solution Explorer` is visible go to `View` > `Solution Explorer`.

To update the Android version for your Android project do the following:

1. Right click on `MyOktaApp.Android` and select `Properties`.
2. In the properties window ensure that the `Application` section is selected.
3. From the dropdown labeled `Compile using Android version: (Target Framework)` select `Android 10.0 (Q)`.

## Configure Your Application

This section describes how to configure your Okta Xamarin application.  These instructions assume you are using `Visual Studio` and were tested with `Visual Studio Community 2019` Version 16.8.0.

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

This section describes the minimal code necessary to handle Okta authentication related redirects when using the Okta Xamarin Sdk.

### Android

To handle Okta authentication redirects on Android do the following:

1. Update your `MainActivity` to extend `OktaMainActivity<App>`.
    ```csharp
    public class MainActivity : OktaMainActivity<App>
    ```
2. Override the OnSignInCompleted and OnSignOutCompleted methods.
    ```csharp
    public override void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
    {
        // for demo purposes go to the profile page
        Shell.Current.GoToAsync("//ProfilePage", true);
    }

    public override void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
    {
        // for demo purposes go to the profile page
        Shell.Current.GoToAsync("//ProfilePage", true);
    }
    ```
3. Your completed `MainActivity.cs` should look similar to the following:
   ```csharp
    [Activity(Label = "MyOktaApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : OktaMainActivity<App>
    {
        public override void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
        {
            // for demo purposes go to the profile page
            Shell.Current.GoToAsync("//ProfilePage", true);
        }

        public override void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
        {
            // for demo purposes go to the profile page
            Shell.Current.GoToAsync("//ProfilePage", true);
        }
    }
   ```
4. Create a new Activity to intercept Login redirects, this example uses `LoginCallbackInterceptorActivity`.
5. Replace the activity implementation with the following code:
    ```csharp
	[Activity(Label = "LoginCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			DataSchemes = new[] { "my.app.login" },
			DataPath = "/callback"
		)
	]
	public class LoginCallbackInterceptorActivity : OktaLoginCallbackInterceptorActivity<MainActivity>
	{
	}
    ```
    > Note that the value specified for `DataSchemes` **MUST** match the prefix entered in step 6 of [Register Redirects](#register-redirects) and the `DataPath` **MUST** match the suffix.
5. Create a new Activity to intercept Logout redirects, this example uses `LogoutCallbackInterceptorActivity`.
6. Replace the activity implementation with the following code:
    ```csharp
	[Activity(Label = "LogoutCallbackInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleInstance)]
	[
		IntentFilter
		(
			actions: new[] { Intent.ActionView },
			Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
			DataSchemes = new[] { "my.app.logout" },
			DataPath = "/callback"
		)
	]
	public class LogoutCallbackInterceptorActivity : OktaLogoutCallbackInterceptorActivity<MainActivity>
	{
	}
    ```
     > Note that the value specified for `DataSchemes` **MUST** match the prefix entered in step 8 of [Register Redirects](#register-redirects) and the `DataPath` **MUST** match the suffix.

### iOS

To handle Okta authentication redirects on iOS do the following:

1. Modify your `AppDelegate` class to extend `OktaAppDelegate<App>`.
2. In the `FinishedLaunching` method add event handlers for the `SignInCompleted` and `SignOutCompleted` events, this example navigates to the `ProfilePage`, you should provide logic appropriate for your application:
    ```csharp
    OktaContext.AddSignInCompletedListener(OnSignInCompleted);
    OktaContext.AddSignOutCompletedListener(OnSignOutCompleted);
    ```
    > A complete AppDelegate example follows:
    ```csharp
    [Register("AppDelegate")]
    public partial class AppDelegate : OktaAppDelegate<App>
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            bool result = base.FinishedLaunching(app, options);
            OktaContext.AddSignInCompletedListener(OnSignInCompleted);
            OktaContext.AddSignOutCompletedListener(OnSignOutCompleted);

            return result;
        }
        
        public void OnSignInCompleted(object sender, SignInEventArgs signInEventArgs)
        {
            // for demo purposes go to the profile page
            Shell.Current.GoToAsync("//ProfilePage", true);
        }
        
        public void OnSignOutCompleted(object sender, SignOutEventArgs signOutEventArgs)
        {
            // for demo purposes go to the profile page
            Shell.Current.GoToAsync("//ProfilePage", true);
        }
    }
    ```

## Refresh Tokens

To receive a refresh token along with id and access tokens do the following:

1. Ensure that `Refresh Token` is checked in the `Allowed grant types` section of your application.
    - Sign in to the admin dashboard for your application.
    - Click on `Applications` and go to `General Settings` > `Application` > `Allowed grant types`.
    - Check the box next to `Refresh Token`.
2. Specify the `offline_access` scope along with the default scopes in your config file.
    - Android:
      - Add the following element inside the `Okta` element of the file `Assets/OktaConfig.xml`.
        ```xml
        <Scope>openid profile offline_access</Scope>
        ```
    - iOS:
      - Add the `Scope` property to the OktaConfig.plist file.
        - Property = `Scope`
        - Type = `String`
        - Value = `openid profile offline_access`

## API Reference

### OktaContext.Current

The `OktaContext.Current` singleton provides a top level entry point into Okta functionality.  

#### SignInAsync

Signs a user in.  Returns a reference to `OktaContext.Current.StateManager` after the sign in process completes. See also, [SignInStarted event](#signinstarted-event) and [SignInCompleted event](#signincompleted-event).
```csharp
await OktaContext.Current.SignInAsync();
```

#### SignOutAsync

Signs a user out. See also, [SignOutStarted event](#signoutstarted-event) and [SignOutCompleted event](#signoutcompleted-event).

```csharp
await OktaContext.Current.SignOutAsync();
```

#### IntrospectAsync

Calls the introspection endpoint to inspect the validity of the specified token. See also, [IntrospectStarted event](#introspectstarted-event) and [IntrospectCompleted event](#introspectcompleted-event).

```csharp
await OktaContext.Current.IntrospectAsync(TokenKind.AccessToken);
```

#### RenewAsync

Since access tokens are traditionally short-lived, you can renew expired tokens by exchanging a refresh token for new ones. 
See [Refresh Tokens](#refresh-tokens) to ensure your app is configured properly for this flow.  See also, [RenewStarted event](#renewstarted-event) and [RenewCompleted event](#renewcompleted-event).

```csharp
OktaContext.Current.RenewAsync(TokenKind.AccessToken);
```

#### RevokeAsync

Calls the revocation endpoint to revoke the specified token kind of token.  See also, [RevokeStarted event](#revokestarted-event) and [RevokeCompleted event](#revokecompleted-event).

```csharp
OktaContext.Current.RevokeAsync();
```

#### GetUserAsync

Calls the OpenID Connect UserInfo endpoint with the stored access token to return user claim information. See also, [GetUserStarted event](#getuserstarted-event) and [GetUserCompleted event](#getusercompleted-event).

```csharp
OktaContext.Current.GetUserAsync();
```

#### Clear

Removes the local authentication state by removing tokens from the state manager. 

```csharp
OktaContext.Clear();
```

#### GetToken

Gets the token of the specified kind from the state manager.

```csharp
OktaContext.GetToken(TokenKind.AccessToken);
```

#### SignInStarted event

The `OktaContext.Current.SignInStarted` event is raised before the login flow begins.  To execute code when the `SignInStarted` event is raised, add an event handler to the `OktaContext.Current.SignInStarted event.  This is done directly or using the static `AddSignInStartedListener` method.

```csharp
// directly
OktaContext.Current.SignInStarted += (sender, signInEventArgs) => Console.WriteLine("SignIn started");

// using AddSignInStartedListener
OktaContext.AddSignInStartedListener((sender, signInEventArgs) => Console.WriteLine("SignIn started"));
```

#### SignInCompleted event

The `OktaContext.Current.SignInCompleted` event is raised when the login flow completes.  To execute code when the `SignInCompleted` event is raised, add an event handler to the `OktaContext.Current.SignInCompleted` event.  This is done directly or using the static `AddSignInCompletedListener` method.

```csharp
// directly
OktaContext.Current.SignInCompleted += (sender, signInEventArgs) => Console.WriteLine("SignIn completed");

// using AddSignInCompletedListener
OktaContext.AddSignInCompletedListener((sender, signInEventArgs) => Console.WriteLine("SignIn completed"));
```

#### SignOutStarted event

The `OktaContext.Current.SignOutStarted` event is raised when the logout flow begins. To execute code when the `SignOutStarted` event is raised, add an event handler to the `OktaContext.Current.SignOutStarted` event.  This is done directly or using the static `AddSignOutStartedListener` method.

```csharp
// directly
OktaContext.Current.SignOutStarted += (sender, signOutEventArgs) => Console.WriteLine("SignOut completed");

// using AddSignOutStartedListener
OktaContext.AddSignOutStartedListener((sender, signOutEventArgs) => Console.WriteLine("SignOut completed"));
```

#### SignOutCompleted event

The `OktaContext.Current.SignOutCompleted` event is raised when the logout flow completes.  To execute code when the `SignOutCompleted` event is raised, add an event handler to the `OktaContext.Current.SignOutCompleted` event.  This is done directly or using the static `AddSignOutCompletedListener` method.

```csharp
// directly
OktaContext.Current.SignOutCompleted += (sender, signOutEventArgs) => Console.WriteLine("SignOut completed");

// using AddSignOutCompletedListener
OktaContext.AddSignOutCompletedListener((sender, signOutEventArgs) => Console.WriteLine("SignOut completed"));
```

#### AuthenticationFailed event

The `OktaContext.Current.AuthenticationFailed` event is raised when an error response is received during the authentication process.  To execute code when the `AuthenticationFailed` event is raised, add an event handler to the `OktaContext.Current.AuthenticationFailed` event.

```csharp
// directly
OktaContext.Current.AuthenticationFailed += (sender, authenticationFailedEventArgs) =>
{
    oAuthException = authenticationFailedEventArgs.OAuthException;

    // ... additional custom logic 
};

// using AddAuthenticationFailedListener
OktaContext.AddAuthenticationFailedListener((sender, authenticationFailedEventArgs) =>
{
    oAuthException = authenticationFailedEventArgs.OAuthException;    

    // ... additional custom logic     
})
```

#### RevokeStarted event

The `OktaContext.Current.RevokeStarted` event is raised before token revocation begins.  To execute code when the `RevokeStarted` event is raised, add an event handler to the `OktaContext.Current.RevokeStarted` event.

```csharp
// directly
OktaContext.Current.RevokeStarted += (sender, revokeEventArgs) =>
{
    string token = revokeEventArgs.Token;
    TokenKind kindOfToken = revokeEventArgs.TokenKind; 

    // ... additional custom logic  
}

// using AddRevokeStartedListener
OktaContext.AddRevokeStartedListener((sender, revokeEventArgs) => 
{
    string token = revokeEventArgs.Token;
    TokenKind kindOfToken = revokeEventArgs.TokenKind; 

    // ... additional custom logic  
})
```

#### RevokeCompleted event

The `OktaContext.Current.RevokeCompleted` event is raised token revocation completes.  To execute code when the `RevokeCompleted` event is raised, add an event handler to the `OktaContext.Current.RevokeCompleted` event.

```csharp
// directly
OktaContext.Current.RevokeCompleted += (sender, revokeEventArgs) =>
{
    string token = revokeEventArgs.Token;
    TokenKind kindOfToken = revokeEventArgs.TokenKind; 
    
    // ... additional custom logic  
}

// using AddRevokeCompletedListener
OktaContext.AddRevokeCompletedListener((sender, revokeEventArgs) => 
{
    string token = revokeEventArgs.Token;
    TokenKind kindOfToken = revokeEventArgs.TokenKind; 

    // ... additional custom logic  
})
```

#### GetUserStarted event

The `OktaContext.Current.GetUserStarted` event is raised before user information is retrieved.  To execute code when the `GetUserStarted` event is raised, add an event handler to the `OktaContext.Current.GetUserStarted` event.

```csharp
// directly
OktaContext.Current.GetUserStarted += (sender, getUserEventArgs) =>
{
    object userInfo = getUserEventArgs.UserInfo; // object type varies depending on which variation of the GetUser method is used

    // ... additional custom logic  
}

// using AddGetUserStartedListener
OktaContext.AddGetUserStartedListener((sender, getUserEventArgs) => 
{
    object userInfo = getUserEventArgs.UserInfo; // object type varies depending on which variation of the GetUser method is used

    // ... additional custom logic  
})
```

#### GetUserCompleted event

The `OktaContext.Current.GetUserCompleted` event is raised after user information is retrieved.  To execute code when the `GetUserCompleted` event is raised, add an event handler to the `OktaContext.Current.GetUserCompleted` event.

```csharp
// directly
OktaContext.Current.GetUserCompleted += (sender, getUserEventArgs) =>
{
    object userInfo = getUserEventArgs.UserInfo; // object type varies depending on which variation of the GetUser method is used

    // ... additional custom logic  
}

// using AddGetUserCompletedListener
OktaContext.AddGetUserCompletedListener((sender, getUserEventArgs) => 
{
    object userInfo = getUserEventArgs.UserInfo; // object type varies depending on which variation of the GetUser method is used

    // ... additional custom logic  
})
```

#### IntrospectStarted event

The `OktaContext.Current.IntrospectStarted` event is raised before token introspection.  To execute code when the `IntrospectStarted` event is raised, add an event handler to the `OktaContext.Current.IntrospectStarted` event.

```csharp
// directly 
OktaContext.Current.IntrospectStarted += (sender, introspectEventArgs) =>
{
    string token = introspectEventArgs.Token;
    TokenKind tokenKind = introspectEventArgs.TokenKind;

    // ... additional custom logic
}

// using AddIntrospectStartedListener
OktaContext.AddIntrospectStartedListener((sender, introspectEventArgs) =>
{
    string token = introspectEventArgs.Token;
    TokenKind tokenKind = introspectEventArgs.TokenKind;

    // ... additional custom logic    
})
```

#### IntrospectCompleted event

The `OktaContext.Current.IntrospectCompleted` event is raised after token introspection.  To execute code when the `IntrospectCompleted` event is raised, add an event handler to the `OktaContext.Current.IntrospectCompleted` event.

```csharp
// directly 
OktaContext.Current.IntrospectCompleted += (sender, introspectEventArgs) =>
{
    string token = introspectEventArgs.Token;
    TokenKind tokenKind = introspectEventArgs.TokenKind;

    // ... additional custom logic
}

// using AddIntrospectCompletedListener
OktaContext.AddIntrospectCompletedListener((sender, introspectEventArgs) =>
{
    string token = introspectEventArgs.Token;
    TokenKind tokenKind = introspectEventArgs.TokenKind;

    // ... additional custom logic    
})
```

#### RenewStarted event

The `OktaContext.Current.RenewStarted` event is raised before token renewal begins.  To execute code when the `RenewStarted` event is raised, add an event handler to the `OktaContext.Current.RenewStarted` event.

```csharp
// directly
OktaContext.Current.RenewStarted += (sender, renewEventArgs) =>
{
    IOktaStateManager stateManager = renewEventArgs.StateManager;

    // ... additional custom logic
}

// using AddRenewStartedListener
OktaContext.AddRenewStartedListener((sender, renewEventArgs) =>
{
    IOktaStateManager stateManager = renewEventArgs.StateManager;

    // ... additional custom logic
})
```

#### RenewCompleted event

The `OktaContext.Current.RenewCompleted` event is raised after token renewal completes.  To execute code when the `RenewCompleted` event is raised, add an event handler to the `OktaContext.Current.RenewCompleted` event.

```csharp
// directly
OktaContext.Current.RenewCompleted += (sender, renewEventArgs) =>
{
    IOktaStateManager stateManager = renewEventArgs.StateManager;

    // ... additional custom logic
}

// using AddRenewCompletedListener
OktaContext.AddRenewCompletedListener((sender, renewEventArgs) =>
{
    IOktaStateManager stateManager = renewEventArgs.StateManager;

    // ... additional custom logic
})
```

### BearerToken and BearerTokenClaims classes

When the `SignInCompleted` event is raised the `EventArgs` parameter instance is of type `SignInEventArgs` which provides access to an instance of the `OktaStateManager` class.  Use code similar to the following to read the authentication state and access the bearer token claims when sign in completes:

```csharp
OktaContext.AddSignInCompletedListener((sender, args) =>
{
    SignInEventArgs signInEventArgs = (SignInEventArgs)args;
    IOktaStateManager oktaStateManager = signInEventArgs.StateManager; //
    BearerToken bearerToken = new BearerToken(oktaStateManager.AccessToken);
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