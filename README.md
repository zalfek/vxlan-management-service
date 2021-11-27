# vxlan-management-service
VXLAN management service for Technische Hochschule Ulm

## Prerequisite

### Register the service app (Web API)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `vxlan-managament-webapi`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **Expose an API** . To declare an resource URI, follow the following steps:
   - Select `Set` next to the **Application ID URI** to generate a URI that is unique for this app.
   - Accept the proposed Application ID URI (`api://{clientId}`) by selecting **Save**.
1. To publish a scope, follow the following steps:
   - Select **Add a scope** button open the **Add a scope** screen and Enter the values as indicated below:
        - For **Scope name**, use `access_as_user`.
        - Select **Admins and users** options for **Who can consent?**.
        - For **Admin consent display name** type `Access vxlan-managament-webapi`.
        - For **Admin consent description** type `Allows the app to access vxlan-managament-webapi as the signed-in user.`
        - For **User consent display name** type `Access vxlan-managament-webapi`.
        - For **User consent description** type `Allow the application to access vxlan-managament-webapi on your behalf.`
        - Keep **State** as **Enabled**.
        - Select the **Add scope** button on the bottom to save this scope.


### Register the client app (Webapp)

1. Navigate to the [Azure portal](https://portal.azure.com) and select the **Azure AD** service.
1. Select the **App Registrations** blade on the left, then select **New registration**.
1. In the **Register an application page** that appears, enter your application's registration information:
   - In the **Name** section, enter a meaningful application name that will be displayed to users of the app, for example `vxlan-managament-webapp`.
   - Under **Supported account types**, select **Accounts in this organizational directory only**.
   - In the **Redirect URI (optional)** section, select **Web** in the combo-box and enter the following redirect URI: `https://localhost:44365/`.
1. Select **Register** to create the application.
1. In the app's registration screen, find and note the **Application (client) ID**.
1. In the app's registration screen, select **Authentication** in the menu.
   - If you don't have a platform added, select **Add a platform** and select the **Web** option.
   - In the **Redirect URIs** section, enter the following redirect URIs.
      - `https://localhost:44365/signin-oidc`
   - In the **Front-channel logout URL** section, set it to `https://localhost:44365/signout-oidc`.
1. Select **Save** to save your changes.
1. In the app's registration screen, select the **Certificates & secrets**
1. In the **Client secrets** section, select **New client secret**:
   - Type a key description (for instance `app secret`),
   - Select one of the available key durations (**In 1 year**, **In 2 years**, or **Never Expires**).
   - The generated key value will be displayed when you select the **Add** button. Note the generated value.
1. In the app's registration screen, select the **API permissions**.
   - Select the **Add a permission** button and then,
   - Ensure that the **My APIs** tab is selected.
   - In the list of APIs, select the API `vxlan-managament-webapi`.
   - In the **Delegated permissions** section, select the **Access 'vxlan-managament-webapi'** in the list. Use the search box if necessary.
   - Select the **Add permissions** button at the bottom.