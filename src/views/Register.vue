<!-- eslint-disable vue/multi-word-component-names -->

<template>
  <div class="container mx-auto p-4">
    <!-- Header for registration page -->
    <h1 class="text-2xl font-bold mb-4">Register</h1>

    <!-- Registration form -->
    <form @submit.prevent="handleRegister">
      <!-- Username input -->
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

      <!-- Email input -->
      <div class="mb-4">
        <label class="block mb-2 font-medium">Email</label>
        <input
          v-model="email"
          type="email"
          class="input"
          placeholder="Enter your email"
          required
        />
      </div>

      <!-- Password input -->
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

      <!-- Confirm password input -->
      <div class="mb-4">
        <label class="block mb-2 font-medium">Confirm Password</label>
        <input
          v-model="confirmPassword"
          type="password"
          class="input"
          placeholder="Confirm your password"
          required
        />
      </div>

      <!-- Register button -->
      <button class="btn btn-primary" type="submit">Register</button>
    </form>

    <!-- Link to login page -->
    <p class="mt-4 text-sm">
      Already a user?
      <router-link to="/login" class="text-blue-500 hover:underline">Login</router-link>
    </p>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import api from '@/utils/api'; // Axios instance for API calls
import axios from 'axios';

export default defineComponent({
  data() {
    return {
      username: '', // Holds the entered username
      email: '', // Holds the entered email
      password: '', // Holds the entered password
      confirmPassword: '', // Holds the re-entered password for confirmation
    };
  },
  methods: {
    async handleRegister() {
      // Check if passwords match before sending data to the backend
      if (this.password !== this.confirmPassword) {
        alert('Passwords do not match!');
        return;
      }

      try {
        // Send registration data to the backend
        const response = await api.post('/auth/register', {
          username: this.username,
          email: this.email, // Include email in the registration request
          password: this.password,
        });

        console.log('Registration successful:', response.data);
        alert('Registration successful! Please login.');
        this.$router.push('/login'); // Redirect to login page on success
      } catch (error) {
        if (axios.isAxiosError(error)) {
          console.error('Axios error:', error.response?.data || error.message);

          // Handle conflict errors like duplicate usernames or emails
          if (error.response?.status === 409) {
            alert(
              error.response?.data?.Error ||
              'A user with this email or username already exists.'
            );
          } else {
            alert('Registration failed. Please try again.');
          }
        } else {
          console.error('Unexpected error:', error);
          alert('An unexpected error occurred. Please try again.');
        }
      }
    },
  },
});
</script>


<!-- Summary
This Vue component provides a user registration form with the following functionalities:

Form Fields: Username, email, password, and confirm password inputs.
Validation: Ensures the password and confirm password fields match before submission.
API Integration: Sends registration data to the backend via the /auth/register endpoint.
Error Handling: Displays appropriate messages for backend errors, such as duplicate users or unexpected issues.
Navigation: Redirects to the login page upon successful registration. -->
