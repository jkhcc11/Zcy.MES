import { printPlugin } from 'vue-print-next'
import { App } from 'vue'
//https://www.npmjs.com/package/vue-print-next
function usePrintNext(app: App) {
  app.use(printPlugin)
}

export default usePrintNext
