<Cabbage>
form caption("Foscili")
hslider channel("freq"), text("Frequency"), range(20, 10000, 440)
hslider channel("mod"), text("Modulation"), range(0, 5, 0, 1, 0.001) bounds(178, 92, 150, 50)
hslider channel("index"), text("Index"), range(0, 30, 0, 1, 0.001) bounds(184, 154, 150, 50)
hslider channel("filterFreq"), text("Filter Frequency"), range(20, 10000, 0, 1, 0.001) bounds(184, 154, 150, 50)
hslider channel("reson"), text("Filter Res"), range(0, 1, 0, 1, 0.001) bounds(184, 154, 150, 50)
hslider channel("dist"), text("Distortion"), range(0, 20, 0, 1, 0.001) bounds(184, 154, 150, 50)
</Cabbage>
<CsoundSynthesizer>
<CsOptions>
; Select audio/midi flags here according to platform
-odac      ;;;realtime audio out
;-iadc    ;;;uncomment -iadc if realtime audio input is needed too
; For Non-realtime ouput leave only the line below:
; -o foscili.wav -W ;;; for file output any platform
</CsOptions>
<CsInstruments>

sr = 44100
ksmps = 32
nchnls = 2
0dbfs  = 1


gaRvbSend init 0

instr 1

kcps = chnget:k("freq")
kcps = port:k(kcps, 0.01)
kcar = 1
kmod = chnget:k("mod")
kmod = port:k(kmod, 0.01)
//kndx line 0, p3, 20	;intensivy sidebands
kndx = chnget:k("index")
kndx = port:k(kndx, 0.01)

//Vibrato
kVibAmp = chnget:k("vib")
kVibAmp = port:k(kVibAmp, 0.01)

kvib = vibr:k(kVibAmp, 4, 1)


kfilterFreq = chnget:k("filterFreq")
kfilterFreq = port:k(kfilterFreq, 0.01)
kfilterBW = chnget:k("reson")
kfilterBW = port:k(kfilterBW, 0.01)
kDistortion = chnget:k("dist")
kDistortion = port:k(kDistortion, 0.01)



asig foscili .1, kcps+kvib, kcar, kmod, kndx, 1

afilter lpf18 asig, kfilterFreq, kfilterBW, kDistortion
     outs afilter, afilter
     
     //sends
     kverbAmt = chnget:k("verb")
     gaRvbSend = gaRvbSend + (asig * kverbAmt)

endin

instr verb
denorm gaRvbSend
kfblvl = chnget:k("feedback")

arvbL, arvbR     reverbsc     gaRvbSend, gaRvbSend, kfblvl, 5000, sr, 1

outs arvbL, arvbR
clear gaRvbSend
endin

</CsInstruments>
<CsScore>
; sine
f 1 0 16384 10 1

i 1 0 z
i "verb" 0 z
e
</CsScore>
</CsoundSynthesizer>






<bsbPanel>
 <label>Widgets</label>
 <objectName/>
 <x>0</x>
 <y>0</y>
 <width>0</width>
 <height>0</height>
 <visible>true</visible>
 <uuid/>
 <bgcolor mode="background">
  <r>240</r>
  <g>240</g>
  <b>240</b>
 </bgcolor>
</bsbPanel>
<bsbPresets>
</bsbPresets>
