<template>
  <div class="container mx-auto p-4">
    <div class="flex justify-between items-center mb-6">
      <h1 class="text-3xl font-bold">Your Notes</h1>
      <button @click="logout" class="btn btn-secondary">Logout</button>
    </div>

    <div class="flex space-x-4 mb-4">
      <input
        v-model="searchQuery"
        type="text"
        class="input"
        placeholder="Search notes by title or content..."
      />
      <select v-model="sortOption" class="input">
        <option value="createdAtAsc">Created At (Ascending)</option>
        <option value="createdAtDesc">Created At (Descending)</option>
        <option value="updatedAtAsc">Updated At (Ascending)</option>
        <option value="updatedAtDesc">Updated At (Descending)</option>
        <option value="titleAsc">Title (A-Z)</option>
        <option value="titleDesc">Title (Z-A)</option>
      </select>
    </div>

    <form @submit.prevent="handleCreateNote" class="mb-6">
      <h2 class="text-2xl font-bold mb-4">Create Note</h2>
      <div class="mb-4">
        <label class="block mb-2 font-medium">Title</label>
        <input v-model="newNote.title" type="text" class="input" required />
      </div>
      <div class="mb-4">
        <label class="block mb-2 font-medium">Content</label>
        <textarea v-model="newNote.content" class="input"></textarea>
      </div>
      <button class="btn btn-primary" type="submit">Create Note</button>
    </form>

    <table class="table-auto w-full border-collapse border border-gray-200">
      <thead>
        <tr>
          <th class="border border-gray-300 px-4 py-2">Title</th>
          <th class="border border-gray-300 px-4 py-2">Content</th>
          <th class="border border-gray-300 px-4 py-2">Created At</th>
          <th class="border border-gray-300 px-4 py-2">Updated At</th>
          <th class="border border-gray-300 px-4 py-2">Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr
          v-for="note in sortedAndFilteredNotes"
          :key="note.id"
          class="hover:bg-gray-100"
        >
          <td class="border border-gray-300 px-4 py-2">{{ note.title }}</td>
          <td class="border border-gray-300 px-4 py-2">{{ note.content }}</td>
          <td class="border border-gray-300 px-4 py-2">
            {{ new Date(note.createdAt).toLocaleString() }}
          </td>
          <td class="border border-gray-300 px-4 py-2">
            {{ new Date(note.updatedAt).toLocaleString() }}
          </td>
          <td class="border border-gray-300 px-4 py-2">
            <button
              @click="deleteNote(note.id)"
              class="btn btn-secondary text-sm"
            >
              Delete
            </button>
            <button
              @click="editNote(note)"
              class="btn btn-primary text-sm ml-2"
            >
              Edit
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import api from '@/utils/api'; // Axios instance to interact with the backend API

// Define the Note interface to structure data
interface Note {
  id: number;
  title: string;
  content: string;
  createdAt: string;
  updatedAt: string;
}

export default defineComponent({
  data() {
    return {
      notes: [] as Note[], // Array to store notes fetched from the backend
      newNote: { title: '', content: '' }, // Data for creating a new note
      searchQuery: '', // Search term entered by the user
      sortOption: 'createdAtAsc', // Default sorting by creation date (ascending)
    };
  },
  computed: {
    // Filter and sort notes based on user input
    sortedAndFilteredNotes(): Note[] {
      const filteredNotes = this.notes.filter(
        (note) =>
          note.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
          note.content.toLowerCase().includes(this.searchQuery.toLowerCase())
      );

      return filteredNotes.sort((a, b) => {
        switch (this.sortOption) {
          case 'createdAtAsc':
            return new Date(a.createdAt).getTime() - new Date(b.createdAt).getTime();
          case 'createdAtDesc':
            return new Date(b.createdAt).getTime() - new Date(a.createdAt).getTime();
          case 'updatedAtAsc':
            return new Date(a.updatedAt).getTime() - new Date(b.updatedAt).getTime();
          case 'updatedAtDesc':
            return new Date(b.updatedAt).getTime() - new Date(a.updatedAt).getTime();
          case 'titleAsc':
            return a.title.localeCompare(b.title);
          case 'titleDesc':
            return b.title.localeCompare(a.title);
          default:
            return 0;
        }
      });
    },
  },
  async created() {
    // Fetch notes when the component is mounted
    await this.fetchNotes();
  },
  methods: {
    async fetchNotes() {
      try {
        const response = await api.get<Note[]>('/notes');
        this.notes = response.data;
      } catch (error) {
        console.error('Error fetching notes:', error);
        alert('Failed to fetch notes.');
      }
    },
    async handleCreateNote() {
      try {
        await api.post('/notes', { ...this.newNote });
        this.newNote = { title: '', content: '' }; // Reset form
        await this.fetchNotes();
      } catch (error) {
        console.error('Error creating note:', error);
        alert('Failed to create note.');
      }
    },
    async deleteNote(id: number) {
      try {
        await api.delete(`/notes/${id}`);
        await this.fetchNotes();
      } catch (error) {
        console.error('Error deleting note:', error);
        alert('Failed to delete note.');
      }
    },
    async editNote(note: Note) {
      const updatedTitle = prompt('Edit Title:', note.title);
      const updatedContent = prompt('Edit Content:', note.content);

      if (updatedTitle && updatedContent) {
        try {
          await api.put(`/notes/${note.id}`, {
            ...note,
            title: updatedTitle,
            content: updatedContent,
          });
          await this.fetchNotes();
        } catch (error) {
          console.error('Error updating note:', error);
          alert('Failed to update note.');
        }
      }
    },
    logout() {
      localStorage.clear();
      this.$router.push('/login');
    },
  },
});
</script>

<style scoped>
.container {
  max-width: 800px;
  margin: 0 auto;
  padding: 20px;
}

.input {
  width: 100%;
  padding: 10px;
  margin-bottom: 10px;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.btn {
  padding: 10px 15px;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.btn-primary {
  background-color: #007bff;
  color: white;
}

.btn-secondary {
  background-color: #6c757d;
  color: white;
}

.table-auto {
  width: 100%;
}

.table-auto th,
.table-auto td {
  padding: 8px;
  text-align: left;
  border-bottom: 1px solid #ddd;
}

.table-auto th {
  background-color: #f2f2f2;
}

.hover\:bg-gray-100:hover {
  background-color: #f0f0f0;
}
</style>