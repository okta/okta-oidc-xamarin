# Changelog
Running changelog of releases since `1.0.0-beta01`

## v3.0.1

## Fixes

- Use scope from configuration on OidcClient.RenewAsync.

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
