[<img src="https://camo.githubusercontent.com/9693e5936d33d830a02cf336dc95874a20958efc/68747470733a2f2f646576666f72756d2e6f6b74612e636f6d2f75706c6f6164732f6f6b74616465762f6f726967696e616c2f31582f626635346131366235666461313839653461643237303666623537636262376131653562386465622e706e67" align="right" width="256px"/>](https://devforum.okta.com/)

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

Before using this SDK you have to create a new object of  `Okta.OidcClient`. You can instantiate  `Okta.OidcClient` w/o parameters that means that SDK will use `Okta.json` for configuration values. Alternatively you can create `Okta.OidcClient` with custom configuration. 

```csharp

// Use the default Okta.config configuration
var oidcClient = new Okta.OidcClient()

// Use configuration from a file
var config = new Okta.Config(/* path to .json file */)

// Specify config manually (it is not recommended to include your ClientSecret directly in source control) 
var config = new Okta.Config(
    Issuer = "https://{yourOktaDomain}/oauth2/default",
    ClientId = "{clientId}",
    ClientSecret = "{clientSecret}",
    RedirectUri = "{redirectUri}",
    LogoutRedirectUri = "{logoutRedirectUri}",
    Scopes = "openid profile offline_access"
)

// Instantiate Okta.OidcClient with custom configuration object
var oidcClient = new Okta.OidcClient(config)

```

**Need a refresh token?**
A refresh token is a special token that is used to generate additional access and ID tokens. Make sure to include the `offline_access` scope in your configuration to silently renew the user's session in your application!

### Okta.json

The easiest way is to create a config file in your solution. By default, this library checks for the existence of the file `Okta.json`. However any json file can be used to create configuration object. Ensure one is created with the following fields:

```json
{
    "issuer": "https://{yourOktaDomain}.com/oauth2/default",
    "clientId": "{clientId}",
    "redirectUri": "{redirectUri}",
    "logoutRedirectUri": "{logoutRedirectUri}",
    "scopes": "openid profile offline_access"
}
```

### Configuration object

Alternatively, you can create a configuration object ( `OktaOidcConfig`) from dictionary with the required values:

```csharp
var config = new Okta.Config(
    Issuer = "https://{yourOktaDomain}/oauth2/default",
    ClientId = "{clientId}",
    ClientSecret = "{clientSecret}",
    RedirectUri = "{redirectUri}",
    LogoutRedirectUri = "{logoutRedirectUri}",
    Scopes = "openid profile offline_access"
)
```

## API Reference

### SignIn()

Start the authorization flow by simply calling `SignIn()`.  This is an async method and should be awaited.  In case of successful authorization, this operation will return valid `Okta.Oidc.StateManager` object. Clients are responsible for further storage and maintenance of the manager.

```csharp
Okta.Oidc.StateManager stateManager = await oidcClient.SignIn();

// stateManager.accessToken
// stateManager.idToken
// stateManager.refreshToken
```

### SignOutOfOkta()

You can start the sign out flow by simply calling `SignOutFromOkta()` with the appropriate `Okta.Oidc.StateManager` . This method will end the user's Okta session in the browser.

**Important**: This method **does not** clear or revoke tokens minted by Okta. Use the [`revoke`](#revoke) and [`clear`](#clear) methods of `Okta.Oidc.StateManager` to terminate the user's local session in your application.

```csharp
// Redirects to the configured 'logoutRedirectUri' specified in the config.
await oidcClient.SignOutOfOkta(stateManager);
```

### Authenticate()

If you already logged in to Okta and have a valid session token, you can complete authorization by calling `Authenticate(sessionToken)`. In case of successful authorization, this operation will return valid `Okta.Oidc.StateManager` in its callback. Clients are responsible for further storage and maintenance of the manager.

```csharp

Okta.Oidc.StateManager stateManager = await oidcClient.Authenticate(sessionToken);

// stateManager.accessToken
// stateManager.idToken
// stateManager.refreshToken
}
```

### StateManager

Tokens are securely stored in the iOS Keychain or the Adroid SecureStore and can be retrieved by accessing the StateManager. 

```csharp
stateManager?.accessToken;
stateManager?.idToken;
stateManager?.refreshToken;
```

The developer is responsible for storing StateManager returned by `SignIn()` or `Authenticate(token)` operation. To store the manager call its `WriteToSecureStorage()` method:

```csharp
var stateManager = await oidcClient.SignIn();
if (stateManager.LoggedIn) {
    stateManager.WriteToSecureStorage()
}
```

To retrieve a stored manager call `ReadFromSecureStorage(config)` and pass in the Okta configuration that corresponds to a manager you are interested in.

```csharp
var stateManager = await Okta.Oidc.StateManager.ReadFromSecureStorage(config);

if (stateManager.LoggedIn) {
    // authenticated 
    // stateManager.accessToken
    // stateManager.idToken
    // stateManager.refreshToken
} else {
    //not authenticated
}
```

**Note:** We support multiple Oauth 2.0 accounts, so a developer can use an Okta endpoint, social endpoint, and others in one application.  The `StateManager` is stored in secure storage seperately based on the specified configuration, which is why the config should be passed into the `ReadFromSecureStorage(config)` method.  If you omit the config parameter it will try to load the default config from an `Okta.json` configuration file.

#### Introspect()

Calls the introspection endpoint to inspect the validity of the specified token.

```csharp
var payload = await stateManager.Introspect(accessToken);

Debug.PrintLine($"Is token valid? {payload.Active}");
```

#### Renew()

Since access tokens are traditionally short-lived, you can renew expired tokens by exchanging a refresh token for new ones. See the [configuration reference](#configuration-reference) to ensure your app is configured properly for this flow.

```csharp
var newAccessToken = await stateManager.Renew();
```

#### Revoke()

Calls the revocation endpoint to revoke the specified token.

```csharp
await stateManager.Revoke(accessToken);
```

#### GetUser()

Calls the OpenID Connect UserInfo endpoint with the stored access token to return user claim information.

```csharp
ClaimsPrincipal user = await stateManager.GetUser();

Debug.PrintLine($"User's name is {user.Identity.Name}");
```

#### Clear()

Removes the local authentication state by removing cached tokens in the keychain.  A full sign out should consist of calling `SignOutOfOkta()` and then `Clear()`.

```csharp
stateManager.Clear();
```


