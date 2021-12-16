name: Bug Report
description: Report a bug you encountered with the Okta Xamarin SDK
labels: [ bug ]
body:
  - type: textarea
    id: problemDescription
    attributes:
      label: Describe the bug?
      description: |
        Please be as detailed as possible. This will help us address the bug in a timely manner.
    validations:
      required: true

  - type: textarea
    id: expectedBehavior
    attributes:
      label: What is expected to happen?
    validations:
      required: true

  - type: textarea
    id: actualBehavior
    attributes:
      label: What is the actual behavior?
    validations:
      required: true

  - type: textarea
    id: reproductionSteps
    attributes:
      label: Reproduction Steps?
      description: |
        Please provide as much detail as possible to help us reproduce the behavior.
        If possible please provide a link to a public repository containing a project or solution that reproduces the behavior.
    validations:
      required: true

  - type: textarea
    id: additionalInformation
    attributes:
      label: Additional Information?

  - type: textarea
    id: dotnetVersion
    attributes:
      label: Dotnet Information
      description: |
        ```powershell
        PS C:\> dotnet --info
        # paste output here
        ```
    validations:
      required: true

  - type: textarea
    id: sdkVersion
    attributes:
      label: SDK Version
      description: |
        ```powershell
        # replace the path with the appropriate path to the Okta.Xamarin.dll on your system
        PS C:\> [System.Reflection.Assembly]::LoadFrom("C:\Okta.Xamarin.dll").GetName().Version
        # paste output here
        ```
    validations:
      required: true

  - type: textarea
    id: osVersion
    attributes:
      label: OS version
      description: |
        ```console
        # On Linux:
        $ cat /etc/os-release

        # On Mac:
        $ uname -a

        # On Windows:
        C:\> wmic os get Caption, Version, BuildNumber, OSArchitecture
        
        # paste output here
        ```