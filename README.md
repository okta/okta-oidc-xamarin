[<img src="https://devforum.okta.com/uploads/oktadev/original/1X/bf54a16b5fda189e4ad2706fb57cbb7a1e5b8deb.png" align="right" width="256px"/>](https://devforum.okta.com/)

# Okta Xamarin OIDC Library

This library is a Xamarin library for communicating with Okta as an OAuth 2.0 + OpenID Connect provider, and follows current best practice for native apps using [Authorization Code Flow + PKCE](https://developer.okta.com/authentication-guide/implementing-authentication/auth-code-pkce).

You can learn more on the [Okta + .NET](https://developer.okta.com/code/xamarin/) page in our documentation.

**Table of Contents**

<!-- TOC depthFrom:2 depthTo:3 -->

[WIP]

<!-- /TOC -->

## Getting Started

Installing the Okta.Xamarin.Oidc SDK into your project is simple. The easiest way to include this library into your project is through [Nuget](https://www.nuget.org/packages/Okta******).  

```cmd
Install-Package Okta.Xamarin.Oidc
```

You'll also need:

- An Okta account, called an _organization_ (sign up for a free [developer organization](https://developer.okta.com/signup/) if you need one).
- An Okta Application, configured as a Native App. This is done from the Okta Developer Console and you can find instructions [here](https://developer.okta.com/authentication-guide/implementing-authentication/auth-code-pkce). When following the wizard, use the default properties. They are designed to work with our sample applications.


## Usage Guide

For an overview of this library's features and authentication flows, check out [our developer docs](https://developer.okta.com/code/****).

<!--
TODO: Once the developer site provides code walkthroughs, update this with a bulleted list of possible flows.
-->

You can also browse the full [API reference documentation](#api-reference).

## Configuration Reference

The entrypoint for the SDK is an instance of `Okta.Oidc.OidcClient`.  If you use the default `OidcClient` constructor without parameters then the SDK will load configuration values from an `Okta.json` file in your project directory.  Alternatively you can create an `OidcClient` with a custom configuration object by passing in a `Okta.Oidc.Config`. 

```csharp

// Use the default configuration from Okta.json
var oidcClient = new Okta.Oidc.OidcClient()

// Load configuration from a file
var config = new Okta.Oidc.Config(/* path to .json file */)

// Specify config manually
var config = new Okta.Oidc.Config(
    OktaDomain = "https://{yourOktaDomain}",
    ClientId = "{clientId}",
    RedirectUri = "{redirectUri}",
    LogoutRedirectUri = "{logoutRedirectUri}",
    Scopes = "openid profile offline_access"
)

// Instantiate Okta.Oidc.OidcClient with a configuration object
var oidcClient = new Okta.Oidc.OidcClient(config)

```

**Need a refresh token?**
A refresh token is a special token that is used to generate additional access and ID tokens. Make sure to include the `offline_access` scope in your configuration to silently renew the user's session in your application!

### Okta.json

***TODO: determine is json is the best option, or if also providing plist (for iOS) and xml (for Android) config files would be more natural***

The easiest way is to create a config file in your solution.  By default, this library checks for the existence of the file `Okta.json`.  However any json file can be used to create configuration object.  Ensure one is created with the following fields:

```json
{
    "Okta":
    {
        "OktaDomain": "https://{yourOktaDomain}",
        "ClientId": "{clientId}",
        "RedirectUri": "{redirectUri}",
        "LogoutRedirectUri": "{logoutRedirectUri}",
        "Scopes": "openid profile offline_access"
    }
}
```

### Configuration object

Alternatively, you can create a configuration object ( `Okta.Oidc.Config`) with the required values:

```csharp
var config = new Okta.Oidc.Config(
    OktaDomain = "https://{yourOktaDomain}",
    ClientId = "{clientId}",
    RedirectUri = "{redirectUri}",
    LogoutRedirectUri = "{logoutRedirectUri}",
    Scopes = "openid profile offline_access"
)
```

## API Reference

### OidcClient

The `Okta.Oidc.OidcClient` class contains methods for signing in, signing out, and authenticating sessions.

#### SignInWithBrowserAsync()

Start the authorization flow by simply calling `SignInWithBrowserAsync()`.  This is an async method and should be awaited.  In case of successful authorization, this operation will return valid `Okta.Oidc.StateManager` object.  Clients are responsible for further storage and maintenance of the manager.

```csharp
Okta.Oidc.StateManager stateManager = await oidcClient.SignInWithBrowserAsync();

// stateManager.AccessToken
// stateManager.IdToken
// stateManager.RefreshToken
```

#### SignOutOfOktaAsync()

You can start the sign out flow by simply calling `SignOutOfOktaAsync()` with the appropriate `Okta.Oidc.StateManager` . This method will end the user's Okta session in the browser.

**Important**: This method **does not** clear or revoke tokens minted by Okta. Use the [`revoke`](#revoke) and [`clear`](#clear) methods of `Okta.Oidc.StateManager` to terminate the user's local session in your application.

```csharp
// Redirects to the configured 'logoutRedirectUri' specified in the config.
await oidcClient.SignOutOfOktaAsync(stateManager);

// to complete sign out, also call `stateManager.RevokeAsync(accessToken)` and then `stateManager.Clear()` 
await stateManager.RevokeAsync(stateManager.AccessToken);
stateManager.Clear();

```

#### AuthenticateAsync()

If you have used the [AuthN SDK](https://github.com/okta/okta-auth-dotnet) to log in to Okta and have a valid session token, you can complete authorization by calling `AuthenticateAsync(sessionToken)`. In case of successful authorization, this operation will return valid `Okta.Oidc.StateManager` in its callback. Clients are responsible for further storage and maintenance of the manager.

```csharp
// pass the sessionToken obtained from the AuthN SDK
Okta.Oidc.StateManager stateManager = await oidcClient.AuthenticateAsync(sessionToken);

// stateManager.AccessToken
// stateManager.IdToken
// stateManager.RefreshToken
}
```

### StateManager

The `SignInWithBrowserAsync()` and `AuthenticateAsync()` operations return an instance of `Okta.Oidc.StateManager`, which includes the login state and any tokens.

```csharp
bool stateManager.IsAuthenticated;
string stateManager.AccessToken;
string stateManager.IdToken;
string stateManager.RefreshToken;
```


#### WriteToSecureStorageAsync()

Tokens are securely stored in the iOS Keychain or the Android SecureStore (***TODO: update with real name***) and can be retrieved by accessing the StateManager.  The developer is responsible for storing StateManager returned by `SignInWithBrowserAsync()` or `AuthenticateAsync(token)` operation.  To store the manager call its `WriteToSecureStorageAsync()` method:

```csharp
var stateManager = await oidcClient.SignInWithBrowserAsync();
if (stateManager.IsAuthenticated) {
    stateManager.WriteToSecureStorageAsync()
}
```

#### ReadFromSecureStorageAsync()

To retrieve a stored manager call `ReadFromSecureStorageAsync(config)` and pass in the Okta configuration that corresponds to a manager you are interested in.

```csharp
var stateManager = await Okta.Oidc.StateManager.ReadFromSecureStorageAsync(config);

if (stateManager.IsAuthenticated) {
    // authenticated 
    // stateManager.AccessToken
    // stateManager.IdToken
    // stateManager.RefreshToken
} else {
    //not authenticated
}
```

**Note:** We support multiple Oauth 2.0 accounts, so a developer can use an Okta endpoint, social endpoint, and others in one application.  The `StateManager` is stored in secure storage separately based on the specified configuration, which is why the config should be passed into the `ReadFromSecureStorageAsync(config)` method if you have multiple accounts.  If you omit the config parameter it will try to load the default config from an `Okta.json` configuration file.

#### IntrospectAsync()

Calls the introspection endpoint to inspect the validity of the specified token.

```csharp
var payload = await stateManager.IntrospectAsync(accessToken);

System.Diagnostics.Debug.WriteLine($"Is token valid? {payload.Active}");
```

#### RenewAsync()

Since access tokens are traditionally short-lived, you can renew expired tokens by exchanging a refresh token for new ones. See the [configuration reference](#configuration-reference) to ensure your app is configured properly for this flow.

```csharp
var newAccessToken = await stateManager.RenewAsync();
```

#### RevokeAsync()

Calls the revocation endpoint to revoke the specified token.  A full sign out should consist of calling `SignOutOfOktaAsync()`, then `RevokeAsync()`, and then `Clear()`.

```csharp
await stateManager.RevokeAsync(accessToken);
```

#### GetUserAsync()

Calls the OpenID Connect UserInfo endpoint with the stored access token to return user claim information.

```csharp
ClaimsPrincipal user = await stateManager.GetUserAsync();

System.Diagnostics.Debug.WriteLine($"User's name is {user.Identity.Name}");
```

#### Clear()

Removes the local authentication state by removing cached tokens in the keychain.  A full sign out should consist of calling `SignOutOfOktaAsync()`, then `RevokeAsync()`, and then `Clear()`.

```csharp
stateManager.Clear();
```


