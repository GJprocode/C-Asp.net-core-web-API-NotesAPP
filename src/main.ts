// Import the main stylesheet for the app
import './assets/main.css';

// Import Vue framework
import { createApp } from 'vue';

// Import Pinia for state management
import { createPinia } from 'pinia';

// Import the main App component
import App from './App.vue';

// Import the router for navigation
import router from './router';

// Create the Vue app instance
const app = createApp(App);

// Add Pinia state management to the app
app.use(createPinia());

// Add the router to manage page navigation
app.use(router);

// Mount the app to the #app element in index.html
app.mount('#app');

/**
 * Summary:
 * - This is the main entry point for the Vue.js application.
 * - It initializes the app, sets up state management with Pinia, and configures routing.
 * - Finally, it mounts the app to the DOM element with the ID `#app`.
 */
