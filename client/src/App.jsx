import { useState } from 'react'
import './App.css'

export default function App() {
  const [number, setNumber] = useState('')
  const [words, setWords] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  function handleChange(e) {
    setNumber(e.target.value)
    setWords('')
    setError('')
  }

  async function handleSubmit(e) {
    e.preventDefault()
    setWords('')
    setError('')
    setLoading(true)

    try {
      const response = await fetch(`/api/convert?number=${encodeURIComponent(number)}`)
      const data = await response.json()

      if (response.ok) {
        setWords(data.words)
      } else {
        setError(data.error)
      }
    } catch {
      setError('Could not reach the server. Please try again.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <main className="card">
      <h1>Number to Words</h1>
      <p className="intro">Enter a dollar amount to see it written out in words.</p>

      <form onSubmit={handleSubmit}>
        <label htmlFor="number">Amount</label>
        <div className="row">
          <input
            id="number"
            value={number}
            onChange={handleChange}
            placeholder="123.45"
            inputMode="decimal"
            autoComplete="off"
            autoFocus
          />
          <button type="submit" disabled={loading}>
            {loading ? 'Converting...' : 'Convert'}
          </button>
        </div>
      </form>

      <div aria-live="polite">
        {error && <p className="message error">{error}</p>}
        {words && <p className="message result">{words}</p>}
      </div>
    </main>
  )
}
