// Import necessary modules and components
import { createRouter, createWebHistory } from 'vue-router'; // For defining and managing routes
import Login from "@/views/LoginPage.vue"; // Component for login page
import Register from "@/views/Register.vue";// Component for registration page
import Notes from '@/views/Notes.vue'; // Component for notes management page

// Define application routes
const routes = [
  { path: '/', redirect: '/login' }, // Redirect root path to login page
  { path: '/login', component: Login }, // Route for login page
  { path: '/register', component: Register }, // Route for registration page
  { path: '/notes', component: Notes }, // Route for notes page
];

// Create a router instance with history mode
const router = createRouter({
  history: createWebHistory(), // Use modern web history for cleaner URLs
  routes, // Assign the defined routes
});

// Export the router instance for use in the application
export default router;

/**
 * Summary:
 * This file sets up the Vue Router for the application. It defines three main routes:
 * - `/login`: For user login.
 * - `/register`: For user registration.
 * - `/notes`: For viewing and managing notes.
 * The root (`/`) route redirects to the `/login` route by default.
 * It uses `createWebHistory()` for clean URLs without hash fragments.
 */
