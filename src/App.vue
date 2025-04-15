<script lang="ts">
export default {
  name: 'App', // The main Vue app component, named "App"
};
</script>

<template>
  <header>
    <!-- Vue logo displayed in the header -->
    <img
      alt="Logo"
      class="logo"
      src="@/assets/logo.svg"
      width="125"
      height="125"
    />
  </header>

  <!-- Router view to dynamically load components based on the route -->
  <RouterView />
</template>

<style scoped>
/* Basic header styling */
header {
  line-height: 1.5; /* Adjusts spacing */
  max-height: 100vh; /* Prevents overflow */
}

/* Styles for the Vue logo */
.logo {
  display: block; /* Centers the logo horizontally */
  margin: 0 auto 2rem; /* Adds bottom margin for spacing */
}

/* Navigation bar styles */
nav {
  width: 100%;
  font-size: 12px; /* Small font size */
  text-align: center; /* Center-aligns text */
  margin-top: 2rem; /* Adds spacing from the top */
}

nav a.router-link-exact-active {
  color: var(--color-text); /* Highlights active link */
}

nav a.router-link-exact-active:hover {
  background-color: transparent; /* Prevents hover effect */
}

/* Default link styles */
nav a {
  display: inline-block;
  padding: 0 1rem; /* Adds horizontal padding */
  border-left: 1px solid var(--color-border); /* Adds left border */
}

nav a:first-of-type {
  border: 0; /* Removes border for the first link */
}

/* Styles for larger screens (desktop) */
@media (min-width: 1024px) {
  header {
    display: flex; /* Aligns header items horizontally */
    place-items: center; /* Centers items vertically */
    padding-right: calc(var(--section-gap) / 2); /* Adds padding on the right */
  }

  .logo {
    margin: 0 2rem 0 0; /* Adds right margin for spacing */
  }

  header .wrapper {
    display: flex;
    place-items: flex-start; /* Aligns content to the top-left */
    flex-wrap: wrap; /* Wraps items when necessary */
  }

  nav {
    text-align: left; /* Aligns text to the left */
    margin-left: -1rem; /* Adjusts left margin */
    font-size: 1rem; /* Increases font size */
    padding: 1rem 0; /* Adds padding */
    margin-top: 1rem; /* Adjusts top margin */
  }
}
</style>


<!-- Summary
This component serves as the main application shell for the Vue project. It includes:

Header with Logo: Displays a centered Vue logo as a visual identity.
Router View: Dynamically loads components based on the route selected by the user.
Responsive Design: Adapts the layout for both small and large screens using CSS media queries.
Scoped Styling: Applies styles locally to this component, avoiding interference with other components. -->
