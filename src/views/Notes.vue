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
