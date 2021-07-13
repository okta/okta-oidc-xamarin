# Changelog
Running changelog of releases since `1.0.0-beta01`

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
