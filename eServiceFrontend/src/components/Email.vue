<template>

<div id="ExportEmailTemplate">
    <table class="wrapper" width="100%" cellspacing="0" cellpadding="0" border="0" bgcolor="#e5e5e5" style="padding-top: 30px; padding-bottom: 30px;" align="center">

       <tr>
           <td class="pad5" valign="top" align="center">
                <table class="wrapper" style="-webkit-border-radius: 5px; -moz-border-radius: 5px; border-radius: 5px;" width="600" cellspacing="0" cellpadding="0" border="0" bgcolor="#fff" align="center">
                  <tr>
                    <td valign="top" align="left">
                      <table class="wrapper" width="600" cellspacing="0" cellpadding="0" border="0" bgcolor="#FFFFFF" align="center">
                        <tr>
                           <td style="padding-bottom: 15px; background: |HEADERBGCOLOR|;" 
                           :style="{'background-color': HeaderBackgroundColor  }" >
                              <table class="wrappermax" width="560" border="0" align="center" cellpadding="0" cellspacing="0">
                                 <tbody>
                                    <tr>
                                      <td>
                                         <table class="wrappermax" align="left" width="250" cellspacing="0" cellpadding="0" border="0">
                                              <tr>
                                                  <td class="centertext" align="left" valign="bottom" style="padding-top: 20px;">
                                                      <a href="https://www.cendyn.com/" target="_blank"><img src="logo.png" alt="|HOTELNAME|" align="bottom" height="52" border="0" /></a>
                                                  </td>
                                              </tr>
                                           </table>

                                          <table width="250" border="0" align="right" cellpadding="0" cellspacing="0" class="wrappermax">
                                              <tr>
                                                  <td class="centertopspace" style="font-family: arial, helvetica, sans serif; font-size: 14px; line-height: 23px; padding-top: 45px; letter-spacing: 0.3px;" valign="top" align="right" 
                                                  :style="{'color': HeaderTextColor  }"> Reservation #: |RESERVATION| </td>
                                              </tr>
                                          </table>  
                                      </td>
                                    </tr>
                                 </tbody>
                              </table>
                           </td>
                        </tr>

                        <tr>
                            <td valign="top" align="center"><img src="header.jpg" alt="|HOTELNAME|" width="600" height="230" class="headerimg" border="0" /></td>
                         </tr>

                         <tr>
                            <td class="pad20" style="padding-bottom: 40px;padding-top: 30px;">
                                <table class="wrapper" width="550" cellspacing="0" cellpadding="0" border="0" align="center">
                                    <tr>
                                        <td class="black" style="font-family: arial, sans-serif; text-decoration:none; font-size:16px; line-height:24px;" 
                                        :style="{'color': TextColor  }" valign="top" align="left">
                                            Dear |FIRSTNAME|,
                                            <br />
                                            <br /> Thank you for your upgrade request!
                                        </td>
                                    </tr>

                                    <tr style="display:|APPROVEDDISPLAYATTR|">
                                        <td class="pad20a" style="font-family: arial, helvetica, sans serif; font-size: 16px;  line-height: 21px; text-align: left; padding: 25px 15px 8px 0;" :style="{'color': TextColor  }">
                                            We are pleased to confirm the following.
                                        </td>
                                    </tr>

                                    <tr>
                                      <td style="padding-bottom:5px; font-family: arial, sans-serif; text-decoration:none; font-size:16px; line-height:24px;"
                                          :style="{'color': TextColor  }" >|CONFIRMEDREQUESTS|
                                      </td>
                                    </tr>

                                    <tr style="display:|DENYDISPLAYATTR|">
                                        <td class="pad20a" style="font-family: arial, helvetica, sans serif; font-size: 16px; line-height: 21px; text-align: left; padding: 25px 15px 8px 0;"
                                            :style="{'color': TextColor  }">
                                            Unfortunately, we are unable to confirm the following due to availability.
                                        </td>
                                    </tr>

                                    <tr>
                                      <td style="padding-bottom:5px; font-family: arial, sans-serif; text-decoration:none; font-size:16px; line-height:24px;"
                                          :style="{'color': TextColor  }">|DENIEDREQUESTS|
                                      </td>
                                    </tr>

                                    <tr>
                                        <td class="black" style="font-family: arial, sans-serif; text-decoration:none; font-size:16px; line-height:24px; padding-top: 10px;" 
                                            :style="{'color': TextColor  }" valign="top" align="left">
                                            We look forward to welcoming you to |HOTELNAME|. Feel free to contact us if you have any questions.
                                            <br />
                                            <br /> Kind regards,

                                            <br /> <a href="|HOTELWEBSITE|" target="_blank" 
                                            :style="{'color': LinksColor  }">|HOTELNAME|</a>
                                            <br /> <font :style="{'color': LinksColor  }">|HOTELEMAIL|</font>
                                            <br /> <font :style="{'color': LinksColor  }">|HOTELPHONE|</font>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                         </tr>
                        <tr>
                          <td :style="{'background-color': FooterBackgroundColor  }">
                              <table class="wrapper" width="550" cellspacing="0" cellpadding="0" border="0" align="center">
                                    <tr>
                                        <td class="pad20a" style="font-family: arial, helvetica, sans serif; font-size: 10px; line-height: 21px; text-align: left; padding: 25px 15px 25px 0;"
                                            :style="{'color': FooterTextColor  }">
                                            @|YEAR| |HOTELNAME|. All rights reserved.
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                      </table>
                    </td>
                  </tr>
                </table>
            </td>
       </tr>
    </table>

</div>

</template>

<script>

export default {
  name: 'Email',
  components: {
    
  },
  data () {
    return {

    }
  },

   computed: {
      TextColor () {
        return this.$store.state.TextColor
      },

      LinksColor () {
        return this.$store.state.LinksColor
      },

      HeaderTextColor () {
        return this.$store.state.HeaderTextColor
      },

      HeaderBackgroundColor() {
        return this.$store.state.HeaderBackgroundColor
      },

      FooterTextColor() {
        return this.$store.state.FooterTextColor
      },

      FooterBackgroundColor() {
        return this.$store.state.FooterBackgroundColor
      },
    },

  methods: {
    
  },
  mounted() {

  },
}
</script>

<style scoped>
        .ExternalClass {
            width: 100%;
        }

            .ExternalClass,
            .ExternalClass p,
            .ExternalClass span,
            .ExternalClass font,
            .ExternalClass td,
            .ExternalClass div {
                line-height: 100%;
            }

        .ReadMsgBody {
            width: 100%;
        }

        .ExternalClass * {
            line-height: 100%;
        }

        body {
            mso-line-height-rule: exactly;
            -webkit-text-size-adjust: 100%;
            -ms-text-size-adjust: 100%;
            line-height: 1;
            border-collapse: collapse;
            border: none;
            border-width: 0;
            margin: 0;
            padding: 0;
        }

        a[x-apple-data-detectors=true] {
            color: inherit !important;
            text-decoration: inherit !important;
        }

        @-ms-viewport {
            width: device-width;
        }

        div,
        p,
        a,
        li,
        td {
            -webkit-text-size-adjust: none;
        }

        table,
        td,
        p {
            mso-table-lspace: 0pt;
            mso-table-rspace: 0pt;
        }

            table td {
                border-collapse: collapse;
            }

        img {
            vertical-align: bottom;
        }

            img.alignment {
                vertical-align: baseline !important;
            }

        span img {
            vertical-align: top;
        }

        .preheader {
            display: none !important;
            visibility: hidden;
            opacity: 0;
            color: transparent;
            height: 0;
            width: 0;
        }

        .links,
        .links a,
        .links a:link,
        .links a:hover,
        .links a:visited {
            color: #28666e !important;
            text-decoration: underline !important;
        }

        .buttonlink,
        .buttonlink a,
        .buttonlink a:link,
        .buttonlink a:hover,
        .buttonlink a:visited {
            color: #ffffff !important;
            text-decoration: none !important;
        }

        .linkbottom,
        .linkbottom a,
        .linkbottom a:link,
        .linkbottom a:hover,
        .linkbottom a:visited {
            color: #6b6b71 !important;
            text-decoration: underline !important;
        }

        @media only screen and (max-width: 640px) {
            table[class="wrapper"] {
                width: 100% !important;
            }

            table[class="wrapper2"] {
                width: 92% !important;
            }

            *[class].wrappermax {
                width: 100% !important;
                text-align: center !important;
            }

            img[class="headerimg"] {
                width: 100% !important;
                height: auto !important;
            }

            td[class="pad20"] {
                padding-right: 20px !important;
                padding-left: 20px !important;
            }

            td[class="pad20a"] {
                padding-right: 20px !important;
                padding-left: 20px !important;
                padding-top: 20px !important;
            }

            td[class="nopad"] {
                padding: 0px !important;
            }

            td[class="padprice"] {
                padding-bottom: 20px !important;
                text-align: center !important;
            }

            td[class="padname"] {
                padding-top: 20px !important;
                text-align: center !important;
            }

            td[class="hideline"] {
                border-left: none !important;
                border-top: solid 1px #cccccc !important;
            }

            td[class="padadjust"] {
                padding-top: 20px !important;
            }

            td[class="hide"] {
                display: none !important;
            }

            br[class="hide"] {
                display: none !important;
            }

            span[class="hide"] {
                display: none !important;
            }

            td[class="centertext"] {
                text-align: center !important;
            }

            td[class="centertopspace"] {
                text-align: center !important;
                padding-top: 10px !important;
            }

            *[class="mob-hide"] {
                display: none !important;
            }
        }

        @media only screen and (max-width:480px) {
            *[class="mob-hide"] {
                display: none !important;
            }
        }

        @media only screen and (max-width: 320px) {
        }
</style>
