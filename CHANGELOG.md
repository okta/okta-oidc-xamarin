# Changelog
Running changelog of releases since `1.0.0-beta01`

## v3.1.1

### Changes 

- Update Xamarin.Forms dependency to version 5.0.0.2515
- Add `OktaPlatform.InitAsync(...)` overloads that accept `UIWindow` instead of `UIViewController` to prevent potential `ObjectDisposedException` that may occur on `SignOut` if `Window.RootViewController` is disposed after initialization
- Deprecate `OktaPlatform.InitAsync(...)` overloads that accept `UIViewController`

## v3.1.0

Encapsulated initialization process.

### Changes

- Update Xamarin.Forms dependency to version 5.0.0.2478
- Update Xamarin.Essentials dependency to version 1.7.3
- Deprecate Okta specific AppDelegate in iOS
- Deprecate Okta specific Activity in Android

### Features

- OktaPlatform class - encapsulates initialization process
- OktaPlatform.InitAsync(...)
- iOS - OktaPlatform.IsOktaCallback(...)
- Android - OktaPlatform.HandleCallback(...)

## v3.0.2

### Fixes

- Use scope from configuration on OidcClient.RenewAsync.

##v3.0.1

### Fixes

- iOS - Ensure CloseBrowser is called on main thread on SignOutComplete
- OktaStateManager.IsAuthenticated - only checks for presence of AccessToken
- OktaStateManager.IsAccessTokenExpired - added

### New Contributors
* @kensykora made their first contribution in https://github.com/okta/okta-oidc-xamarin/pull/73

## v3.0.0

Update/correct handling of authorization server Id.  Conveniences added and internal structure changes made to support future additions and extensions.

### Fixes

- Check StateManager for null on sign out
- Don't fire SignInCompleted event on OAuthException

### Changes

- Refactored initialization process
- Make demo application a submodule

### Features

- Added convenience methods to OktaContext to manage loading and saving state.
  - OktaContext.SaveStateAsync()
  - OktaContext.LoadStateAsync()
- Added convenience methods for token revocation
  - OktaContext.RevokeAccessToken(accessToken)
  - OktaContext.RevokeRefreshToken(refreshToken)
- Added initialization related events to OktaContext
  - InitServicesStarted
  - InitServicesCompleted
  - InitServicesException
- Added secure storage related events to OktaContext
  - LoadStateStarted
  - LoadStateCompleted
  - LoadStateException
  - SecureStorageWriteStarted
  - SecureStorageWriteCompleted
  - SecureStorageWriteException
  - SecureStorageReadStarted
  - SecureStorageReadCompleted
  - SecureStorageReadException
- Added token related exception events to OktaContext
  - RevokeException
  - RenewException
- Added TinyIoCContainer
- Added convenience methods to IOktaStateManager
  - GetAccessToken()
  - GetRefreshToken()
  - GetIdToken()

## v2.0.0

Cleanup and continued refinement of existing features.

- Removed demo application from primary binaries
- Fixed Revoke & Renew implementations

## v1.0.0

This is the first stable release of the Okta Xamarin Sdk. 
### Features

- Added OktaContext convenience methods and events:
  - Events
    - SignInStarted
    - SignInCompleted
    - SignOutStarted
    - SignOutCompleted    
    - RevokeStarted
    - RevokeCompleted
    - GetUserStarted
    - GetUserCompleted
    - IntrospectStarted
    - IntrospectCompleted
    - RenewStarted
    - RenewCompleted
  - Methods
    - SignInAsync
    - SignOutAsync
    - RevokeAsync
    - GetUserAsync
    - IntrospectAsync
    - RenewAsync
- Implemented OktaStateManager token lifecycle methods:
  - GetToken
  - RenewAsync
  - RevokeAsync

## v1.0.0-beta02

### Features

- SignOut
- Xamarin.Forms compatibility

### Additions

- New class:  `OktaContext`

## v1.0.0-beta01

### Features

- SignIn
