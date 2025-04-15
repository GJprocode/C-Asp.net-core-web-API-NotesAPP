<!-- eslint-disable vue/multi-word-component-names -->

<template>
  <div class="container mx-auto p-4">
    <!-- Page heading -->
    <h1 class="text-2xl font-bold mb-4">Welcome to the NotesAPP!</h1>
    <h2 class="text-2xl font-bold mb-4">Login</h2>

    <!-- Login form -->
    <form @submit.prevent="handleLogin">
      <div class="mb-4">
        <label class="block mb-2 font-medium">Username</label>
        <input
          v-model="username"
          type="text"
          class="input"
          placeholder="Enter your username"
          required
        />
      </div>
      <div class="mb-4">
        <label class="block mb-2 font-medium">Password</label>
        <input
          v-model="password"
          type="password"
          class="input"
          placeholder="Enter your password"
          required
        />
      </div>
      <!-- Submit button -->
      <button class="btn btn-primary w-full" type="submit">Login</button>
    </form>

    <!-- Link to registration page -->
    <p class="mt-4 text-sm text-center">
      Not registered yet?
      <router-link to="/register" class="text-blue-500 hover:underline">Sign up here</router-link>.
    </p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import axios from 'axios'; // Axios for handling HTTP requests
import api from '@/utils/api'; // Custom Axios instance for API interactions

export default defineComponent({
  // Form data for login
  data() {
    return {
      username: '', // Stores the entered username
      password: '', // Stores the entered password
    };
  },
  methods: {
    // Handle the login form submission
    async handleLogin() {
      try {
        // Send login request to the backend API
        const response = await api.post('/auth/login', {
          username: this.username,
          password: this.password,
        });

        // Save the JWT token locally and navigate to the notes page
        localStorage.setItem('token', response.data.token);
        alert('Login successful!');
        this.$router.push('/notes');
      } catch (error) {
        // Handle errors from Axios or general exceptions
        if (axios.isAxiosError(error)) {
          console.error('Axios error:', error.response?.data || error.message);
          alert(error.response?.data?.Error || 'Login failed. Please check your credentials.');
        } else if (error instanceof Error) {
          console.error('General error:', error.message);
          alert('An unexpected error occurred. Please try again.');
        } else {
          console.error('Unexpected error:', error);
          alert('Unexpected error occurred.');
        }
      }
    },
  },
});
</script>

<style scoped>
/* Style for the container */
.container {
  max-width: 400px;
  margin: 0 auto;
}

/* Style for input fields */
.input {
  border: 1px solid #ccc;
  padding: 0.5rem;
  width: 100%;
  border-radius: 4px;
  margin-top: 0.25rem;
}

/* Styles for buttons */
.btn {
  padding: 0.5rem 1rem;
  border-radius: 4px;
  font-weight: bold;
  text-align: center;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-primary:hover {
  background-color: #0056b3;
}

/* Styles for text links */
.text-blue-500 {
  color: #007bff;
}

.text-blue-500:hover {
  text-decoration: underline;
}
</style>


 <!-- * Summary:
 * - This Vue component renders the login page for the NotesApp.
 * - Users enter their username and password and submit the form.
 * - The `handleLogin` method sends the credentials to the backend API and stores the JWT token on success.
 * - Upon successful login, users are redirected to the notes page.
 * - Includes basic styling with scoped CSS and links to a registration page for new users. -->


