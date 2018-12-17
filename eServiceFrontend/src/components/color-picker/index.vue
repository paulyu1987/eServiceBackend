<template lang="html">
    <div class="m-colorPicker" ref="colorPicker" v-on:click="event => { event.stopPropagation() }">
        
        <div class="colorBtn"
             v-bind:style="`background-color: ${showColor}`"
             v-on:click="openStatus = !disabled" 
             v-bind:class="{ disabled: disabled }"></div>
        
        <input type="color"
               ref="html5Color"
               v-model="html5Color"
               v-on:change="updataValue(html5Color)">
        
        <div class="box" v-bind:class="{ open: openStatus }">
            <div class="hd">
                <div class="colorView" v-bind:style="`background-color: ${showPanelColor}`"></div>
                <div class="defaultColor"
                     v-on:click="handleDefaultColor"
                     v-on:mouseover="hoveColor = defaultColor"
                     v-on:mouseout="hoveColor = null"
                >Close
                </div>
            </div>
            <div class="bd">
                <h3>Theme Color</h3>
                <ul class="tColor">
                    <li
                            v-for="color of tColor"
                            v-bind:style="{ backgroundColor: color }"
                            v-on:mouseover="hoveColor = color"
                            v-on:mouseout="hoveColor = null"
                            v-on:click="updataValue(color)"
                    ></li>
                </ul>
                <ul class="bColor">
                    <li v-for="item of colorPanel">
                        <ul>
                            <li
                                    v-for="color of item"
                                    v-bind:style="{ backgroundColor: color }"
                                    v-on:mouseover="hoveColor = color"
                                    v-on:mouseout="hoveColor = null"
                                    v-on:click="updataValue(color)"
                            ></li>
                        </ul>
                    </li>
                </ul>
                <h3>Standard Colors</h3>
                <ul class="tColor">
                    <li
                            v-for="color of bColor"
                            v-bind:style="{ backgroundColor: color }"
                            v-on:mouseover="hoveColor = color"
                            v-on:mouseout="hoveColor = null"
                            v-on:click="updataValue(color)"
                    ></li>
                </ul>
                <!--<h3 v-on:click="triggerHtml5Color">More Color...</h3>-->
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        name: 'colorPicker',
        props: {
            // Current color
            value: {
                type: String,
                required: true
            },
            // Default color
            defaultColor: {
                type: String,
                default: '#000'
            },
            // Disabled state
            disabled: {
                type: Boolean,
                default: false
            }
         
        },
        data () {
            return {
                // Panel display status
                openStatus: false,
                // mouse hover color
                hoveColor: null,
                // theme color
                tColor: ['#000', '#fff', '#eeece1', '#1e497b', '#4e81bb', '#e2534d', '#9aba60', '#8165a0', '#47acc5', '#f9974c'],
                // color panel
                colorConfig: [
                    ['#7f7f7f', '#f2f2f2'],
                    ['#0d0d0d', '#808080'],
                    ['#1c1a10', '#ddd8c3'],
                    ['#0e243d', '#c6d9f0'],
                    ['#233f5e', '#dae5f0'],
                    ['#632623', '#f2dbdb'],
                    ['#4d602c', '#eaf1de'],
                    ['#3f3150', '#e6e0ec'],
                    ['#1e5867', '#d9eef3'],
                    ['#99490f', '#fee9da']
                ],
                // Standard Colors
                bColor: ['#c21401', '#ff1e02', '#ffc12a', '#ffff3a', '#90cf5b', '#00af57', '#00afee', '#0071be', '#00215f', '#72349d'],
                html5Color: this.value
            }
        },
        computed: {
            // display panel color
            showPanelColor () {
                if (this.hoveColor) {
                    return this.hoveColor
                } else {
                    return this.showColor
                }
            },
            // show color
            showColor () {
                if (this.value) {
                    return this.value
                } else {
                    return this.defaultColor
                }
            },
            // color panel
            colorPanel () {
                let colorArr = []
                for (let color of this.colorConfig) {
                    colorArr.push(this.gradient(color[1], color[0], 5))
                }
                return colorArr
            }
        },
        methods: {
            triggerHtml5Color () {
                this.$refs.html5Color.click()
            },
            // Update the value of the component.
            updataValue (value) {
                this.$emit('input', value)
                this.$emit('change', value)
                this.openStatus = false
            },
            // Set default color
            handleDefaultColor () {
//                this.updataValue(this.defaultColor)
                this.openStatus = false
            },
            // Format the hex color.
            parseColor (hexStr) {
                if (hexStr.length === 4) {
                    hexStr = '#' + hexStr[1] + hexStr[1] + hexStr[2] + hexStr[2] + hexStr[3] + hexStr[3]
                } else {
                    return hexStr
                }
            },
            // RGB to HEX
            rgbToHex (r, g, b) {
                let hex = ((r << 16) | (g << 8) | b).toString(16)
                return '#' + new Array(Math.abs(hex.length - 7)).join('0') + hex
            },
            // HEX to RGB
            hexToRgb (hex) {
                hex = this.parseColor(hex)
                let rgb = []
                for (let i = 1; i < 7; i += 2) {
                    rgb.push(parseInt('0x' + hex.slice(i, i + 2)))
                }
                return rgb
            },
            // Calculate the gradient transition color.
            gradient (startColor, endColor, step) {
                // HEX to RGB
                let sColor = this.hexToRgb(startColor)
                let eColor = this.hexToRgb(endColor)

                // Calculate the difference of each step of R\G\B.
                let rStep = (eColor[0] - sColor[0]) / step
                let gStep = (eColor[1] - sColor[1]) / step
                let bStep = (eColor[2] - sColor[2]) / step

                let gradientColorArr = []
                // Calculate the hex value for each step.
                for (let i = 0; i < step; i++) {
                    gradientColorArr.push(this.rgbToHex(parseInt(rStep * i + sColor[0]), parseInt(gStep * i + sColor[1]), parseInt(bStep * i + sColor[2])))
                }
                return gradientColorArr
            }
        },
        mounted () {
            // Click on the rest of the page and close the popover.
            document.onclick = (e) => {
                this.openStatus = false
            }
        }
    }
</script>

<style scoped>
    .m-colorPicker {
        position: absolute;
        text-align: left;
        font-size: 14px;
        display: inline-block;
    }

    .m-colorPicker ul, .m-colorPicker li, .m-colorPicker ol {
        list-style: none;
        margin: 0;
        padding: 0;
    }

    .m-colorPicker input {
        display: none;
    }

    .m-colorPicker .colorBtn {
        width: 30px;
        height: 30px;
        border: 1px solid #DEE0E7;
        margin-top: 2px;
        margin-left: 15px;
    }

    .m-colorPicker .colorBtn.disabled {
        cursor: no-drop;
    }

    .m-colorPicker .box {
        position: absolute;
        width: 250px;
        background: #fff;
        border: 1px solid #ddd;
        visibility: hidden;
        border-radius: 2px;
        margin-top: 2px;
        padding: 10px;
        padding-bottom: 5px;
        box-shadow: 0 0 5px rgba(0, 0, 0, 0.15);
        opacity: 0;
        transition: all .3s ease;
    }

    .m-colorPicker .box h3 {
        margin: 0;
        font-size: 14px;
        font-weight: normal;
        margin-top: 10px;
        margin-bottom: 5px;
        line-height: 1;
    }

    .m-colorPicker .box.open {
        visibility: visible;
        opacity: 1;
        z-index: 2000;
    }

    .m-colorPicker .hd {
        overflow: hidden;
        line-height: 29px;
    }

    .m-colorPicker .hd .colorView {
        width: 100px;
        height: 30px;
        float: left;
        transition: background-color .3s ease;
    }

    .m-colorPicker .hd .defaultColor {
        width: 80px;
        float: right;
        text-align: center;
        border: 1px solid #ddd;
        cursor: pointer;
    }

    .m-colorPicker .tColor li {
        width: 15px;
        height: 15px;
        display: inline-block;
        margin: 0 2px;
        transition: all .3s ease;
    }

    .m-colorPicker .tColor li:hover {
        box-shadow: 0 0 5px rgba(0, 0, 0, 0.4);
        transform: scale(1.3);
    }

    .m-colorPicker .bColor li {
        width: 15px;
        display: inline-block;
        margin: 0 2px;
    }

    .m-colorPicker .bColor li li {
        display: block;
        width: 15px;
        height: 15px;
        transition: all .3s ease;
        margin: 0;
    }

    .m-colorPicker .bColor li li:hover {
        box-shadow: 0 0 5px rgba(0, 0, 0, 0.4);
        transform: scale(1.3);
    }

</style>
