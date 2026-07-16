<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import { HubConnectionBuilder } from '@microsoft/signalr'

type DeploymentEvent = { deploymentId: string; timestamp: string; status: string; message: string; nodeId?: string }
const events = ref<DeploymentEvent[]>([])
const connected = ref(false)
const summary = computed(() => `${events.value.length} live event${events.value.length === 1 ? '' : 's'}`)

onMounted(() => {
  const connection = new HubConnectionBuilder().withUrl('/hubs/deployment').withAutomaticReconnect().build()
  connection.on('deploymentEvent', (event: DeploymentEvent) => events.value.unshift(event))
  connection.onreconnecting(() => { connected.value = false })
  connection.onreconnected(() => { connected.value = true })
  connection.start().then(() => { connected.value = true }).catch(() => { connected.value = false })
})
</script>

<template>
  <main>
    <header>
      <div><strong>DeploymentCenter</strong><span> Enterprise release control plane</span></div>
      <span :class="{ online: connected }">{{ connected ? 'Live' : 'Connecting' }}</span>
    </header>
    <section class="cards">
      <article><h2>Applications</h2><p>.NET 10 and Vue 3 packages</p></article>
      <article><h2>Strategies</h2><p>Single, rolling, blue-green</p></article>
      <article><h2>Activity</h2><p>{{ summary }}</p></article>
    </section>
    <section class="events">
      <h1>Deployment stream</h1>
      <p v-if="!events.length">Waiting for deployment events…</p>
      <ol v-else>
        <li v-for="event in events" :key="`${event.deploymentId}-${event.timestamp}`">
          <time>{{ new Date(event.timestamp).toLocaleTimeString() }}</time>
          <b>{{ event.status }}</b> {{ event.message }}
        </li>
      </ol>
    </section>
  </main>
</template>
